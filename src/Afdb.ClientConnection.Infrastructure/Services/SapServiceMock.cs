using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
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

    public Task<List<SapProjectData>> GetProjectsByCountryAsync(string countryCode, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_projects.Select(p => new SapProjectData {
            projectDescription = p.ProjectName,
            ProjectCode = p.SapCode,
            ProjectTitle = p.ProjectName,
            CountryCode = p.CountryCode })
         .Where(p => string.Equals(p.CountryCode, countryCode, StringComparison.OrdinalIgnoreCase)).ToList());
    }
}
