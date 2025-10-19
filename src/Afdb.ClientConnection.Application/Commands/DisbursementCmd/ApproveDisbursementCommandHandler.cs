using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class ApproveDisbursementCommandHandler(
    IDisbursementRepository disbursementRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<ApproveDisbursementCommand, ApproveDisbursementResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;

    public async Task<ApproveDisbursementResponse> Handle(ApproveDisbursementCommand request, CancellationToken cancellationToken)
    {
        var disbursement = await _disbursementRepository.GetByIdAsync(request.DisbursementId, cancellationToken)
            ?? throw new NotFoundException("ERR.Disbursement.NotFound");

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        disbursement.Approve(user.Id, _currentUserService.Email);

        var updatedDisbursement = await _disbursementRepository.UpdateAsync(disbursement, cancellationToken);

        return new ApproveDisbursementResponse
        {
            Disbursement = _mapper.Map<DTOs.DisbursementDto>(updatedDisbursement),
            Message = "MSG.DISBURSEMENT.APPROVED_SUCCESS"
        };
    }
}
