using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface ISapService
{
    Task<IEnumerable<ProjectDto>> GetProjectsAsync(CancellationToken cancellationToken = default);
    Task<List<SapProjectData>> GetProjectsByCountryAsync(string countryCode, CancellationToken cancellationToken = default);
}
