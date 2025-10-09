using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByEntraIdObjectIdAsync(string entraIdObjectId);
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
    Task<IEnumerable<User>> GetActiveUsersByRolesAsync(List<UserRole> roles);
}