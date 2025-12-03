using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed class GetUserDisbursementPermissionsQueryHandler(
    ICurrentUserService currentUserService,
    IUserRepository userRepository,
    IAccessRequestRepository accessRequestRepository,
    IDisbursementPermissionRepository permissionRepository) : IRequestHandler<GetUserDisbursementPermissionsQuery, DisbursementPermissionsDto>
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IDisbursementPermissionRepository _permissionRepository = permissionRepository;
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;

    public async Task<DisbursementPermissionsDto> Handle(
        GetUserDisbursementPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        var accessRequest = await _accessRequestRepository.GetByEmailAsync(user.Email)
                    ?? throw new NotFoundException("ERR.Disbursement.AccessRequestNotFound");


        if (accessRequest.FunctionId == null || accessRequest.BusinessProfileId == null)
        {
            return new DisbursementPermissionsDto
            {
                CanConsult = false,
                CanSubmit = false
            };
        }

        var permission = await _permissionRepository.GetByBusinessProfileAndFunctionAsync(
            accessRequest.BusinessProfileId.Value,
            accessRequest.FunctionId.Value,
            cancellationToken);

        if (permission == null)
        {
            return new DisbursementPermissionsDto
            {
                CanConsult = false,
                CanSubmit = false
            };
        }

        return new DisbursementPermissionsDto
        {
            CanConsult = permission.CanConsult,
            CanSubmit = permission.CanSubmit
        };
    }
}
