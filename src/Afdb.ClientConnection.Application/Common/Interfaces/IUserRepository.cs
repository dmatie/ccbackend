using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByEntraIdObjectIdAsync(string entraIdObjectId);
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
    Task<IEnumerable<User>> GetActiveUsersByRolesAsync(List<UserRole> roles);
    Task<int> CountByRoleAsync(List<UserRole> roles, CancellationToken cancellationToken = default);
    Task<User> AddInternalAsync(User user, AzureAdUserDetails adUserDetails, UserRole userRole, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetActiveInternalUsersAsync();
}