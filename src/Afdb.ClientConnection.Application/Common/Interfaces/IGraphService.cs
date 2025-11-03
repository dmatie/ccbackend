using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IGraphService
{
    Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<string> CreateGuestUserAsync(string email, string firstName, string lastName,
        string? organizationName, CancellationToken cancellationToken = default);
    Task<bool> DeleteGuestUserAsync(string objectId, CancellationToken cancellationToken = default);
    Task<List<string>> GetFifcAdmin(CancellationToken cancellationToken = default);
    Task<List<string>> GetFifcDOs(CancellationToken cancellationToken = default);
    Task<List<string>> GetFifcDAs(CancellationToken cancellationToken = default);
    Task<List<AzureAdUserDetails>> SearchUsersAsync(string searchQuery, int maxResults = 10, CancellationToken cancellationToken = default);
    Task<AzureAdUserDetails?> GetAzureAdUserDetailsAsync(string email, CancellationToken cancellationToken);
    Task AssignAppRoleToUserByRoleNameAsync(string userId, UserRole userRole, CancellationToken cancellationToken = default);
    Task AddUserToGroupByNameAsync(string userId, UserRole userRole, CancellationToken cancellationToken = default);
    Task RemoveUserFromGroupByNameAsync(string userId, UserRole userRole, CancellationToken cancellationToken = default);
    Task RemoveAppRoleFromUserByRoleNameAsync(string userId, UserRole userRole, CancellationToken cancellationToken = default);
}
