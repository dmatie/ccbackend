using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed class GetDisbursementsByUserQueryHandler(
    IDisbursementRepository disbursementRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetDisbursementsByUserQuery, IEnumerable<DisbursementDto>>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DisbursementDto>> Handle(GetDisbursementsByUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        var disbursements = await _disbursementRepository.GetByUserIdAsync(user.Id, cancellationToken);
        return _mapper.Map<IEnumerable<DisbursementDto>>(disbursements);
    }
}
