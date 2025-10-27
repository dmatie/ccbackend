using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class SubmitDisbursementCommandHandler(
    IDisbursementRepository disbursementRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<SubmitDisbursementCommand, SubmitDisbursementResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<SubmitDisbursementResponse> Handle(SubmitDisbursementCommand request, CancellationToken cancellationToken)
    {
        var disbursement = await _disbursementRepository.GetByIdAsync(request.DisbursementId, cancellationToken)
            ?? throw new NotFoundException("ERR.Disbursement.NotFound");

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        disbursement.Submit(user);

        var updatedDisbursement = await _disbursementRepository.UpdateProcessAsync(disbursement, cancellationToken);

        return new SubmitDisbursementResponse
        {
            Disbursement = _mapper.Map<DTOs.DisbursementDto>(updatedDisbursement),
            Message = "MSG.DISBURSEMENT.SUBMITTED_SUCCESS"
        };
    }
}
