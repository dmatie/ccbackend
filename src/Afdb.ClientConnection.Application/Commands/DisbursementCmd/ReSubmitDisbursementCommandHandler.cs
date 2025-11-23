using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Helpers;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class ReSubmitDisbursementCommandHandler(
    IDisbursementRepository disbursementRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IFileValidationService fileValidationService,
    IDisbursementDocumentService disbursementDocumentService,
    IUserContextService userContextService,
    ICountryAdminRepository countryAdminRepository,
    IGraphService graphService,
    IMapper mapper) : IRequestHandler<ReSubmitDisbursementCommand, ReSubmitDisbursementResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IFileValidationService _fileValidationService = fileValidationService;
    private readonly IDisbursementDocumentService _disbursementDocumentService = disbursementDocumentService;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly ICountryAdminRepository _countryAdminRepository = countryAdminRepository;
    private readonly IGraphService _graphService = graphService;
    private readonly IMapper _mapper = mapper;

    public async Task<ReSubmitDisbursementResponse> Handle(ReSubmitDisbursementCommand request, CancellationToken cancellationToken)
    {
        var userContext = _userContextService.GetUserContext();

        if (request.AdditionalDocuments != null && request.AdditionalDocuments.Count > 0)
        {
            await _fileValidationService.ValidateAndThrowAsync(request.AdditionalDocuments, "AdditionalDocuments");
        }

        var disbursement = await _disbursementRepository.GetByIdAsync(request.DisbursementId, cancellationToken)
            ?? throw new NotFoundException("ERR.Disbursement.NotFound");

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        string[] fifcAdmins = (await _graphService.GetFifcAdmin(cancellationToken)).ToArray() ?? [];
        string[] assignTo = [];

        if (userContext.IsExternal && userContext.AccessRequest is not null && userContext.AccessRequest.CountryId is not null)
        {
            var accessRequestCountryAdmins = await _countryAdminRepository
                .GetByCountryIdAsync(userContext.AccessRequest.CountryId.Value, cancellationToken);

            if (accessRequestCountryAdmins is not null && accessRequestCountryAdmins.Any())
            {
                assignTo = accessRequestCountryAdmins.Select(ca => ca.User!.Email).ToArray();
            }
        }

        if (assignTo.Length == 0)
        {
            assignTo = fifcAdmins;
        }
        else
        {
            assignTo = fifcAdmins;
        }

        disbursement.Resubmit(user, request.Comment, assignTo,fifcAdmins);

        if (request.AdditionalDocuments != null && request.AdditionalDocuments.Count > 0)
        {
            await _disbursementDocumentService.UploadAndAttachDocumentsAsync(
                disbursement,
                request.AdditionalDocuments,
                cancellationToken);
        }
        else
        {
            await _disbursementRepository.UpdateProcessAsync(disbursement, cancellationToken);
        }

        var updatedDisbursement = await _disbursementRepository.GetByIdAsync(disbursement.Id, cancellationToken);

        return new ReSubmitDisbursementResponse
        {
            Disbursement = _mapper.Map<DTOs.DisbursementDto>(updatedDisbursement),
            Message = "MSG.DISBURSEMENT.RESUBMITTED_SUCCESS"
        };
    }
}
