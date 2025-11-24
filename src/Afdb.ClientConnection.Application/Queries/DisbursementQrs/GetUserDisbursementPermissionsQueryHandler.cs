using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed class GetUserDisbursementPermissionsQueryHandler(
    ICurrentUserService currentUserService,
    IUserRepository userRepository,
    IDisbursementPermissionRepository permissionRepository) : IRequestHandler<GetUserDisbursementPermissionsQuery, DisbursementPermissionsDto>
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IDisbursementPermissionRepository _permissionRepository = permissionRepository;

    public async Task<DisbursementPermissionsDto> Handle(
        GetUserDisbursementPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email);

        if (user?.FunctionId == null || user?.BusinessProfileId == null)
        {
            return new DisbursementPermissionsDto
            {
                CanConsult = false,
                CanSubmit = false
            };
        }

        var permission = await _permissionRepository.GetByBusinessProfileAndFunctionAsync(
            user.BusinessProfileId.Value,
            user.FunctionId.Value,
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
