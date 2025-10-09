namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IGraphService
{
    Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<string> CreateGuestUserAsync(string email, string firstName, string lastName,
        string? organizationName, CancellationToken cancellationToken = default);
    Task<bool> DeleteGuestUserAsync(string objectId, CancellationToken cancellationToken = default);
    Task<List<string>> GetFifcAdmin(CancellationToken cancellationToken = default);
}
