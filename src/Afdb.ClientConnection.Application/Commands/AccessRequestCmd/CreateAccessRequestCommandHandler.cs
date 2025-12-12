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
    IAuditService auditService,
    IReferenceService referenceService,
    IMapper mapper) : IRequestHandler<CreateAccessRequestCommand, CreateAccessRequestResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IGraphService _graphService = graphService;
    private readonly IAuditService _auditService = auditService;
    private readonly IMapper _mapper = mapper;
    private readonly IReferenceService _referenceService = referenceService;


    public async Task<CreateAccessRequestResponse> Handle(CreateAccessRequestCommand command, CancellationToken cancellationToken)
    {
        // Vérifier si l'utilisateur existe déjà dans notre système
        var existingUser = await _userRepository.GetByEmailAsync(command.Email);
        if (existingUser != null)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.AccessRequest.EmailAlreadyExists")
            });
        }

        // Vérifier si une demande est déjà en cours pour cet email
        var existingRequest = await _accessRequestRepository.GetByEmailAsync(command.Email);
        if (existingRequest != null)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.AccessRequest.RequestAlreadyExists")
            });
        }

        // Vérifier si l'utilisateur existe dans Entra ID
        var userExistsInEntraId = await _graphService.UserExistsAsync(command.Email, cancellationToken);
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

        if (command.FunctionId.HasValue)
            function = await _referenceService.GetFunctionByIdAsync(command.FunctionId.Value, cancellationToken) ??
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Function", "ERR.General.ReferenceDataNotExist")
            });

        if (command.BusinessProfileId.HasValue)
            businessProfile = await _referenceService.GetBusinessProfileByIdAsync(command.BusinessProfileId.Value, cancellationToken) ??
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("BusinessProfile", "ERR.General.ReferenceDataNotExist")
            });

        if (command.CountryId.HasValue)
            country = await _referenceService.GetCountryByIdAsync(command.CountryId.Value, cancellationToken) ??
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Country", "ERR.General.ReferenceDataNotExist")
            });

        if (command.FinancingTypeId.HasValue)
            financingType = await _referenceService.GetFinancingTypeByIdAsync(command.FinancingTypeId.Value, cancellationToken);

        List<AccessRequestProject> projects = [];
        if (command.Projects.Count > 0)
        {
            foreach (var project in command.Projects)
            {
                projects.Add(new(Guid.Empty, project.SapCode, project.ProjectTitle));
            }
        }

        List<string> approvers = await _graphService.GetFifcAdmin(cancellationToken);
        if (approvers == null || approvers.Count == 0)
            throw new NotFoundException("ERR.General.MissingAdGroup");

        string uniqueCode = await _accessRequestRepository.GenerateUniqueCodeAsync(cancellationToken);

        var accessRequest = new AccessRequest(new Domain.EntitiesParams.AccessRequestNewParam()
        {
            Code = uniqueCode,
            ApproversEmail = approvers.ToArray(),
            BusinessProfile = businessProfile,
            BusinessProfileId = command.BusinessProfileId,
            CountryId = command.CountryId,
            Country = country,
            CreatedBy = "System",
            Email = command.Email,
            FinancingType = financingType,
            FinancingTypeId = command.FinancingTypeId,
            FirstName = command.FirstName,
            Function = function,
            FunctionId = command.FunctionId,
            LastName = command.LastName,
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
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                FunctionId = command.FunctionId,
                CountryId = command.CountryId,
                BusinessProfileId = command.BusinessProfileId
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
