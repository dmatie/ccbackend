using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class BackToClientDisbursementCommandHandler(
    IDisbursementRepository disbursementRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<BackToClientDisbursementCommand, BackToClientDisbursementResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;

    public async Task<BackToClientDisbursementResponse> Handle(BackToClientDisbursementCommand request, CancellationToken cancellationToken)
    {
        var disbursement = await _disbursementRepository.GetByIdAsync(request.DisbursementId, cancellationToken)
            ?? throw new NotFoundException("ERR.Disbursement.NotFound");

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        disbursement.BackToClient(user, request.Comment);

        var updatedDisbursement = await _disbursementRepository.UpdateProcessAsync(disbursement, cancellationToken);

        return new BackToClientDisbursementResponse
        {
            Disbursement = _mapper.Map<DTOs.DisbursementDto>(updatedDisbursement),
            Message = "MSG.DISBURSEMENT.BACKED_TO_CLIENT_SUCCESS"
        };
    }
}
