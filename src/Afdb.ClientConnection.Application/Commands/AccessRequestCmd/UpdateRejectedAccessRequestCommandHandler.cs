using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed class UpdateRejectedAccessRequestCommandHandler(
    IAccessRequestRepository accessRequestRepository,
    IUserRepository userRepository,
    IGraphService graphService,
    ICurrentUserService currentUserService,
    IAuditService auditService,
    IReferenceService referenceService,
    IMapper mapper) : IRequestHandler<UpdateRejectedAccessRequestCommand, UpdateRejectedAccessRequestResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IGraphService _graphService = graphService;
    private readonly IAuditService _auditService = auditService;
    private readonly IMapper _mapper = mapper;
    private readonly IReferenceService _referenceService = referenceService;


    public async Task<UpdateRejectedAccessRequestResponse> Handle(UpdateRejectedAccessRequestCommand request, CancellationToken cancellationToken)
    {
        // Vérifier si l'utilisateur existe déjà dans notre système
        var existingRejectRequest = await _accessRequestRepository.GetByEmailAndStatusAsync(request.Email,Domain.Enums.RequestStatus.Rejected);
        if (existingRejectRequest is null)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.AccessRequest.RejectedRequestNotExist")
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
        if(approvers == null || approvers.Count == 0)
            throw new NotFoundException("ERR.General.MissingAdGroup");

        existingRejectRequest.Update(new Domain.EntitiesParams.AccessRequestNewParam()
        {
            ApproversEmail = approvers.ToArray(),
            BusinessProfile = businessProfile,
            BusinessProfileId = request.BusinessProfileId,
            CountryId = request.CountryId,
            Country = country,
            CreatedBy = "System",
            Email = request.Email,
            FinancingType = financingType,
            FinancingTypeId = request.FinancingTypeId,
            FirstName = request.FirstName,
            Function = function,
            FunctionId = request.FunctionId,
            LastName = request.LastName,
            Projects = projects
        });

        // Sauvegarder
        await _accessRequestRepository.UpdateAsync(existingRejectRequest);

        //Logger l'audit
        await _auditService.LogAsync(
            nameof(CreateAccessRequestCommand),
            existingRejectRequest.Id,
            "Update",
            oldValues: System.Text.Json.JsonSerializer.Serialize(new
            {
                Email = existingRejectRequest.Email,
                FirstName = existingRejectRequest.FirstName,
                LastName = existingRejectRequest.LastName,
                FunctionId = existingRejectRequest.FunctionId,
                CountryId = existingRejectRequest.CountryId,
                BusinessProfileId = existingRejectRequest.BusinessProfileId,
                projects = existingRejectRequest.Projects.Select(p => p.SapCode).ToList()
            }),
            newValues: System.Text.Json.JsonSerializer.Serialize(new
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                FunctionId = request.FunctionId,
                CountryId = request.CountryId,
                BusinessProfileId = request.BusinessProfileId,
                projects = projects.Select(p => p.SapCode).ToList()
            }),
            cancellationToken);

        var dto = _mapper.Map<AccessRequestDto>(existingRejectRequest);

        return new UpdateRejectedAccessRequestResponse
        {
            AccessRequest = dto,
            Message = "MSG.AccessRequestSubmitted"
        };
    }
}
