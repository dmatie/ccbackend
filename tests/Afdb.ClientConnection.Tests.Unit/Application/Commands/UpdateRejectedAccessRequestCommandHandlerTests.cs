using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using Moq;

namespace Afdb.ClientConnection.Tests.Unit.Application.Commands;

public class UpdateRejectedAccessRequestCommandHandlerTests
{
    private readonly Mock<IAccessRequestRepository> _accessRequestRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IGraphService> _graphServiceMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly Mock<IAuditService> _auditServiceMock = new();
    private readonly Mock<IReferenceService> _referenceServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private UpdateRejectedAccessRequestCommandHandler CreateHandler()
    {
        return new UpdateRejectedAccessRequestCommandHandler(
            _accessRequestRepositoryMock.Object,
            _userRepositoryMock.Object,
            _graphServiceMock.Object,
            _currentUserServiceMock.Object,
            _auditServiceMock.Object,
            _referenceServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldUpdateRejectedAccessRequest_WhenValid()
    {
        // Arrange

        var request = new UpdateRejectedAccessRequestCommand
        {
            Email = "testupdate@afdb.org",
            FirstName = "Test",
            LastName = "update",
            FunctionId = Guid.NewGuid(),
            CountryId = Guid.NewGuid(),
            BusinessProfileId = Guid.NewGuid(),
            FinancingTypeId = Guid.NewGuid(),
            Projects = new List<UpdateRequestProject>
            {
                new() { SapCode = "PRJ001" }
            }
        };

        var function = new Function(request.FunctionId.Value, "ADB Desk Office", "ADBDESK", "African Development Bank Desk Office", "system");
        var country = new Country(request.CountryId.Value, "Algeria", "Algérie", "DZA", "system");
        var businessProfile = new BusinessProfile(request.BusinessProfileId.Value, "Executing Agency", "Executing Agency", "system");
        var financingType = new FinancingType(request.FinancingTypeId.Value, "Private", "Privé", "system");


        var accessRequest = new AccessRequest(new AccessRequestNewParam
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            FunctionId = request.FunctionId,
            CountryId = request.CountryId,
            BusinessProfileId = request.BusinessProfileId,
            FinancingTypeId = request.FinancingTypeId,
            Projects = new List<AccessRequestProject> { new(Guid.NewGuid(), "PRJ001") }
        });

        _accessRequestRepositoryMock
            .Setup(r => r.GetByEmailAndStatusAsync(request.Email, RequestStatus.Rejected))
            .ReturnsAsync(accessRequest);

        _graphServiceMock
            .Setup(g => g.UserExistsAsync(request.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _referenceServiceMock
            .Setup(r => r.GetFunctionByIdAsync(request.FunctionId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(function);

        _referenceServiceMock
            .Setup(r => r.GetBusinessProfileByIdAsync(request.BusinessProfileId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(businessProfile);

        _referenceServiceMock
            .Setup(r => r.GetCountryByIdAsync(request.CountryId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(country);

        _referenceServiceMock
            .Setup(r => r.GetFinancingTypeByIdAsync(request.FinancingTypeId.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(financingType);

        _graphServiceMock
            .Setup(g => g.GetFifcAdmin(It.IsAny<CancellationToken>()))
            .ReturnsAsync(["admin@afdb.org"]);

        _accessRequestRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<AccessRequest>()))
            .Returns(Task.CompletedTask);

        _auditServiceMock
            .Setup(a => a.LogAsync(
                It.IsAny<string>(),
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var expectedDto = new AccessRequestDto { Id = accessRequest.Id };
        _mapperMock.Setup(x => x.Map<AccessRequestDto>(It.IsAny<AccessRequest>()))
            .Returns(expectedDto);


        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result.AccessRequest);
        Assert.Equal("MSG.AccessRequestSubmitted", result.Message);

        _accessRequestRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AccessRequest>()), Times.Once);
        _auditServiceMock.Verify(a => a.LogAsync(
            nameof(CreateAccessRequestCommand),
            It.IsAny<Guid>(),
            "Update",
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}