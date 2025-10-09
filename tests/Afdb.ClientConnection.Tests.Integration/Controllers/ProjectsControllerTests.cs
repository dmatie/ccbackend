using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Queries.ProjectQrs;
using System.Net;
using System.Text.Json;

namespace Afdb.ClientConnection.Tests.Integration.Controllers;

[Collection("IntegrationTestCollection")]
public class ProjectsControllerTests
{
    private readonly HttpClient _client;

    public ProjectsControllerTests(CustomWebApplicationFactoryFixture fixture)
    {
        _client = fixture.Factory.CreateClient();
    }

    [Fact]
    public async Task GetProjects_ReturnsOkAndProjects()
    {
        var response = await _client.GetAsync("/api/projects");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<GetProjectsByCountryResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


        Assert.NotNull(result);
        Assert.NotEmpty(result.Projects);
    }

    [Fact]
    public async Task GetProjectsByCountry_ReturnsOkAndProjects()
    {
        var response = await _client.GetAsync("/api/projects/country/ZA");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<GetProjectsByCountryResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


        Assert.NotNull(result);
        Assert.All(result.Projects, p => Assert.Equal("ZA", p.CountryCode));
    }
}
