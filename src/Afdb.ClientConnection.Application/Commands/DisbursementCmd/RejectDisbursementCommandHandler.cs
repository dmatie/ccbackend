using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class RejectDisbursementCommandHandler(
    IDisbursementRepository disbursementRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<RejectDisbursementCommand, RejectDisbursementResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;

    public async Task<RejectDisbursementResponse> Handle(RejectDisbursementCommand request, CancellationToken cancellationToken)
    {
        var disbursement = await _disbursementRepository.GetByIdAsync(request.DisbursementId, cancellationToken)
            ?? throw new NotFoundException("ERR.Disbursement.NotFound");

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        disbursement.Reject(user, request.Comment);

        var updatedDisbursement = await _disbursementRepository.UpdateProcessAsync(disbursement, cancellationToken);

        return new RejectDisbursementResponse
        {
            Disbursement = _mapper.Map<DTOs.DisbursementDto>(updatedDisbursement),
            Message = "MSG.DISBURSEMENT.REJECTED_SUCCESS"
        };
    }
}
