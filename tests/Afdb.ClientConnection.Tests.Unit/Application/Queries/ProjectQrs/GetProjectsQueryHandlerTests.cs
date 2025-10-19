using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Application.Queries.ProjectQrs;
using Moq;


namespace Afdb.ClientConnection.Tests.Unit.Application.Queries.ProjectQrs;

public sealed class GetProjectsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsProjectsResponse()
    {
        // Arrange
        var mockSapService = new Mock<ISapService>();
        var projects = new List<ProjectDto>
        {
            new ProjectDto { SapCode = "P-ZA-F00-001", ProjectName = "Projet 1", CountryCode = "ZA", ManagingCountryCode = "ZA" }
        };
        mockSapService.Setup(s => s.GetProjectsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);

        var handler = new GetProjectsQueryHandler(mockSapService.Object);

        // Act
        var response = await handler.Handle(new GetProjectsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.Projects);
        Assert.Equal("P-ZA-F00-001", response.Projects[0].SapCode);
    }
}