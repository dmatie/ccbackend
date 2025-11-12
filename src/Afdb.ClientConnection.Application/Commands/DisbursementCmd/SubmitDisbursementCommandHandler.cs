using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class SubmitDisbursementCommandHandler(
    IDisbursementRepository disbursementRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    ICountryAdminRepository countryAdminRepository,
    IGraphService graphService,
    IUserContextService userContextService,
    IMapper mapper) : IRequestHandler<SubmitDisbursementCommand, SubmitDisbursementResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICountryAdminRepository _countryAdminRepository = countryAdminRepository;
    private readonly IGraphService _graphService = graphService;
    private readonly IUserContextService _userContextService = userContextService;

    private readonly IMapper _mapper = mapper;

    public async Task<SubmitDisbursementResponse> Handle(SubmitDisbursementCommand request, CancellationToken cancellationToken)
    {
        var userContext = _userContextService.GetUserContext();

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

        disbursement.Submit(user, assignTo, fifcAdmins);

        var updatedDisbursement = await _disbursementRepository.UpdateProcessAsync(disbursement, cancellationToken);

        return new SubmitDisbursementResponse
        {
            Disbursement = _mapper.Map<DTOs.DisbursementDto>(updatedDisbursement),
            Message = "MSG.DISBURSEMENT.SUBMITTED_SUCCESS"
        };
    }
}
