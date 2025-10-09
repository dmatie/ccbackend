using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using System.Text.Json;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class SapServiceMock : ISapService
{
    private static List<ProjectDto>_projects = [];

    public SapServiceMock()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "mock-projects.json");
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            var projects =  JsonSerializer.Deserialize<List<ProjectDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if(projects is not null)
                _projects.Clear();
                _projects.AddRange(projects!);

        }
    }

    public Task<IEnumerable<ProjectDto>> GetProjectsAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IEnumerable<ProjectDto>>(_projects);

    public Task<ProjectDto?> GetProjectAsync(string sapId, CancellationToken cancellationToken = default)
        => Task.FromResult(_projects.FirstOrDefault(p => p.SapCode == sapId));

    public Task<IEnumerable<ProjectDto>> GetProjectsByCountryAsync(string countryCode, CancellationToken cancellationToken = default)
        => Task.FromResult(_projects.Where(p => string.Equals(p.CountryCode, countryCode, StringComparison.OrdinalIgnoreCase)));
}
