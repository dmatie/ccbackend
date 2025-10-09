using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class SapService : ISapService
{
    public Task<ProjectDto?> GetProjectAsync(string sapId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProjectDto>> GetProjectsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<ProjectDto>> GetProjectsByCountryAsync(string countryCode, CancellationToken cancellationToken = default)
    {
        var allProjects = await GetProjectsAsync(cancellationToken);
        return allProjects.Where(p => string.Equals(p.CountryCode, countryCode, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
