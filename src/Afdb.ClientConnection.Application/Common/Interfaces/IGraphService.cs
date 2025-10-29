using Afdb.ClientConnection.Application.DTOs;

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
    Task<List<AzureAdUserDto>> SearchUsersAsync(string searchQuery, int maxResults = 10, CancellationToken cancellationToken = default);
}
