using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Application.Queries.ProjectQrs;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Afdb.ClientConnection.Tests.Unit.Application.Queries.ProjectQrs;

public class GetProjectsByCountryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsProjectsForCountry()
    {
        var mockService = new Mock<ISapService>();
        var projects = new List<ProjectDto>
        {
            new ProjectDto { SapCode = "P-ZA-F00-001", ProjectName = "Projet ZA", CountryCode = "ZA", ManagingCountryCode = "ZA" },
            new ProjectDto { SapCode = "P-BJ-F00-002", ProjectName = "Projet BJ", CountryCode = "BJ", ManagingCountryCode = "BJ" }
        };
        mockService.Setup(s => s.GetProjectsByCountryAsync("ZA", It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects.Where(p => p.CountryCode == "ZA"));

        var handler = new GetProjectsByCountryQueryHandler(mockService.Object);

        var result = await handler.Handle(new GetProjectsByCountryQuery("ZA"), CancellationToken.None);

        Assert.Single(result.Projects);
        Assert.Equal("ZA", result.Projects[0].CountryCode);
    }
}