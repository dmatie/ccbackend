using Afdb.ClientConnection.Application.DTOs;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface ISapService
{
    Task<IEnumerable<ProjectDto>> GetProjectsAsync(CancellationToken cancellationToken = default);
    Task<ProjectDto?> GetProjectAsync(string sapId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProjectDto>> GetProjectsByCountryAsync(string countryCode, CancellationToken cancellationToken = default);
}
