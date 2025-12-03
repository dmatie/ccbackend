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
    IDisbursementPermissionRepository permissionRepository,
    IAccessRequestRepository accessRequestRepository,
    IMapper mapper) : IRequestHandler<GetDisbursementsByUserQuery, IEnumerable<DisbursementDto>>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IDisbursementPermissionRepository _permissionRepository = permissionRepository;
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DisbursementDto>> Handle(GetDisbursementsByUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");


        var accessRequest = await _accessRequestRepository.GetByEmailAsync(user.Email)
                    ?? throw new NotFoundException("ERR.Disbursement.AccessRequestNotFound");


        if (!accessRequest.FunctionId.HasValue)
        {
            var userDisbursements = await _disbursementRepository.GetByUserIdAsync(user.Id, cancellationToken);
            return _mapper.Map<IEnumerable<DisbursementDto>>(userDisbursements);
        }

        var authorizedBusinessProfileIds = await _permissionRepository
          .GetAuthorizedBusinessProfileIdsAsync(accessRequest.FunctionId.Value, cancellationToken);

        var disbursements = await _disbursementRepository
            .GetByUserIdWithPermissionsAsync(user.Id, authorizedBusinessProfileIds, cancellationToken);


        return _mapper.Map<IEnumerable<DisbursementDto>>(disbursements);
    }
}
