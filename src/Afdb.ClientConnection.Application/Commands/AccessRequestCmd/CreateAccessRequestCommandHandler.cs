using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed class CreateAccessRequestCommandHandler(
    IAccessRequestRepository accessRequestRepository,
    IUserRepository userRepository,
    IGraphService graphService,
    ICurrentUserService currentUserService,
    IAuditService auditService,
    IReferenceService referenceService,
    IMapper mapper) : IRequestHandler<CreateAccessRequestCommand, CreateAccessRequestResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IGraphService _graphService = graphService;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAuditService _auditService = auditService;
    private readonly IMapper _mapper = mapper;
    private readonly IReferenceService _referenceService = referenceService;


    public async Task<CreateAccessRequestResponse> Handle(CreateAccessRequestCommand request, CancellationToken cancellationToken)
    {
        // Vérifier si l'utilisateur existe déjà dans notre système
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.AccessRequest.EmailAlreadyExists")
            });
        }

        // Vérifier si une demande est déjà en cours pour cet email
        var existingRequest = await _accessRequestRepository.GetByEmailAsync(request.Email);
        if (existingRequest != null && existingRequest.Status == Domain.Enums.RequestStatus.Pending)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.AccessRequest.RequestAlreadyExists")
            });
        }

        // Vérifier si l'utilisateur existe dans Entra ID
        var userExistsInEntraId = await _graphService.UserExistsAsync(request.Email, cancellationToken);
        if (userExistsInEntraId)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.General.EmailExistsInEntra")
            });
        }

        Function? function = null;
        BusinessProfile? businessProfile = null;
        Country? country = null;
        FinancingType? financingType = null;

        if (request.FunctionId.HasValue)
            function = await _referenceService.GetFunctionByIdAsync(request.FunctionId.Value, cancellationToken) ??
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Function", "ERR.General.ReferenceDataNotExist")
            });

        if (request.BusinessProfileId.HasValue)
            businessProfile = await _referenceService.GetBusinessProfileByIdAsync(request.BusinessProfileId.Value, cancellationToken) ??
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("BusinessProfile", "ERR.General.ReferenceDataNotExist")
            });

        if (request.CountryId.HasValue)
            country = await _referenceService.GetCountryByIdAsync(request.CountryId.Value, cancellationToken)??
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Country", "ERR.General.ReferenceDataNotExist")
            });

        if (request.FinancingTypeId.HasValue)
            financingType = await _referenceService.GetFinancingTypeByIdAsync(request.FinancingTypeId.Value, cancellationToken);

        List<AccessRequestProject> projects = [];
        if (request.Projects.Count > 0)
        {
            foreach (var project in request.Projects) 
            {
                projects.Add(new(Guid.Empty, project.SapCode));
            }
        }

        List<string> approvers = await _graphService.GetFifcAdmin(cancellationToken);
        if (approvers == null || approvers.Count == 0)
            throw new NotFoundException("ERR.General.MissingAdGroup");


        var accessRequest = new AccessRequest(new Domain.EntitiesParams.AccessRequestNewParam() 
        {
            ApproversEmail= approvers.ToArray(),
            BusinessProfile= businessProfile,
            BusinessProfileId= request.BusinessProfileId,
            CountryId= request.CountryId,
            Country = country,
            CreatedBy= "System",
            Email= request.Email,
            FinancingType= financingType,
            FinancingTypeId = request.FinancingTypeId,
            FirstName= request.FirstName,
            Function= function,
            FunctionId= request.FunctionId,
            LastName= request.LastName, 
            Projects = projects
        });

        // Sauvegarder
        await _accessRequestRepository.AddAsync(accessRequest);

        //Logger l'audit
        await _auditService.LogAsync(
            nameof(CreateAccessRequestCommand),
            accessRequest.Id,
            "Create",
            null,
            newValues: System.Text.Json.JsonSerializer.Serialize(new
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                FunctionId = request.FunctionId,
                CountryId = request.CountryId,
                BusinessProfileId = request.BusinessProfileId
            }),
            cancellationToken);

        var dto = _mapper.Map<AccessRequestDto>(accessRequest);

        return new CreateAccessRequestResponse
        {
            AccessRequest = dto,
            Message = "MSG.AccessRequestSubmitted"
        };
    }
}
