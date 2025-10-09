using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using Moq;

namespace Afdb.ClientConnection.Tests.Unit.Application.Commands;

public class CreateAccessRequestCommandHandlerTests
{
    private readonly Mock<IAccessRequestRepository> _mockAccessRequestRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IGraphService> _mockGraphService;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<IAuditService> _mockAuditService;
    private readonly Mock<IReferenceService> _mockReferenceService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateAccessRequestCommandHandler _handler;

    public CreateAccessRequestCommandHandlerTests()
    {
        _mockAccessRequestRepository = new Mock<IAccessRequestRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockGraphService = new Mock<IGraphService>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockAuditService = new Mock<IAuditService>();
        _mockReferenceService = new Mock<IReferenceService>();
        _mockMapper = new Mock<IMapper>();

        _handler = new CreateAccessRequestCommandHandler(
            _mockAccessRequestRepository.Object,
            _mockUserRepository.Object,
            _mockGraphService.Object,
            _mockCurrentUserService.Object,
            _mockAuditService.Object,
            _mockReferenceService.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesAccessRequest()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            FunctionId = Guid.NewGuid(),
            CountryId = Guid.NewGuid(),
            BusinessProfileId = Guid.NewGuid()
        };

        var function = new Function(command.FunctionId.Value, "ADB Desk Office", "ADBDESK", "African Development Bank Desk Office", "system");
        var country = new Country(command.CountryId.Value,"Algeria", "Algérie", "DZA", "system");
        var businessProfile = new BusinessProfile(command.BusinessProfileId.Value, "Executing Agency", "Executing Agency", "system");

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        _mockAccessRequestRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((AccessRequest?)null);

        _mockGraphService.Setup(x => x.UserExistsAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockGraphService.Setup(g => g.GetFifcAdmin(It.IsAny<CancellationToken>()))
            .ReturnsAsync(["admin@afdb.org"]);

        _mockCurrentUserService.Setup(x => x.IsAuthenticated).Returns(false);
        _mockCurrentUserService.Setup(x => x.UserId).Returns("SOM13980");

        // Setup reference service mocks
        _mockReferenceService.Setup(x => x.GetFunctionByIdAsync(command.FunctionId!.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(function);

        _mockReferenceService.Setup(x => x.GetBusinessProfileByIdAsync(command.BusinessProfileId!.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(businessProfile);

        _mockReferenceService.Setup(x => x.GetCountryByIdAsync(command.CountryId!.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(country);

        var expectedAccessRequest = new AccessRequest(
            new AccessRequestNewParam()
            {
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                CreatedBy = "SOM13980",
                FunctionId= command.FunctionId,
                CountryId= command.CountryId,
                BusinessProfileId= command.BusinessProfileId,
                Function=function,
                Country=country,
                BusinessProfile=businessProfile,
                Projects = []

            });

        _mockAccessRequestRepository.Setup(x => x.AddAsync(It.IsAny<AccessRequest>()))
            .ReturnsAsync(expectedAccessRequest);

        var expectedDto = new AccessRequestDto { Id = expectedAccessRequest.Id };
        _mockMapper.Setup(x => x.Map<AccessRequestDto>(It.IsAny<AccessRequest>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result.AccessRequest);
        Assert.Equal("MSG.AccessRequestSubmitted", result.Message);

        _mockAccessRequestRepository.Verify(x => x.AddAsync(It.IsAny<AccessRequest>()), Times.Once);
        _mockAuditService.Verify(x => x.LogAsync(
            nameof(CreateAccessRequestCommand),
            It.IsAny<Guid>(),
            "Create",
            null,
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyExists_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "existing@example.com",
            FirstName = "John",
            LastName = "Doe",
            FunctionId = Guid.NewGuid()
        };

        var existingUser = new User(
            command.Email, "John", "Doe", UserRole.ExternalUser,
            "entra-123", "system");

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.NotNull(exception.Errors);
        Assert.NotEmpty(exception.Errors);
        Assert.Contains("ERR.AccessRequest.EmailAlreadyExists", exception.Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task Handle_WhenPendingRequestExists_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "pending@example.com",
            FirstName = "John",
            LastName = "Doe",
            CountryId = Guid.NewGuid()
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        var existingRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email= command.Email,
            FirstName= command.FirstName,
            LastName= command.LastName,
            CreatedBy="System"
        });

        _mockAccessRequestRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(existingRequest);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.NotNull(exception.Errors);
        Assert.NotEmpty(exception.Errors);
        Assert.Contains("ERR.AccessRequest.RequestAlreadyExists", exception.Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task Handle_WhenUserExistsInEntraId_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "entraid@example.com",
            FirstName = "John",
            LastName = "Doe",
            BusinessProfileId = Guid.NewGuid()
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        _mockAccessRequestRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((AccessRequest?)null);

        _mockGraphService.Setup(x => x.UserExistsAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.NotNull(exception.Errors);
        Assert.NotEmpty(exception.Errors);
        Assert.Contains("ERR.General.EmailExistsInEntra", exception.Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task Handle_WithAllOptionalIds_CreatesAccessRequestWithAllRelations()
    {
        // Arrange
        var functionId = Guid.NewGuid();
        var countryId = Guid.NewGuid();
        var businessProfileId = Guid.NewGuid();

        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            FunctionId = functionId,
            CountryId = countryId,
            BusinessProfileId = businessProfileId
        };

        var function = new Function(command.FunctionId.Value, "ADB Desk Office","ADBDESK" , "African Development Bank Desk Office", "system");
        var country = new Country(command.CountryId.Value, "Algeria", "Algérie", "DZA", "system");
        var businessProfile = new BusinessProfile(command.BusinessProfileId.Value, "Executing Agency", "Executing Agency", "system");

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        _mockAccessRequestRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((AccessRequest?)null);

        _mockGraphService.Setup(x => x.UserExistsAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockGraphService.Setup(g => g.GetFifcAdmin(It.IsAny<CancellationToken>()))
            .ReturnsAsync(["admin@afdb.org"]);

        _mockCurrentUserService.Setup(x => x.IsAuthenticated).Returns(true);
        _mockCurrentUserService.Setup(x => x.UserId).Returns("SOM13980");

        // Setup reference service mocks
        _mockReferenceService.Setup(x => x.GetFunctionByIdAsync(functionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(function);

        _mockReferenceService.Setup(x => x.GetBusinessProfileByIdAsync(businessProfileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(businessProfile);

        _mockReferenceService.Setup(x => x.GetCountryByIdAsync(countryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(country);

        var expectedAccessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email=command.Email,
            FirstName=command.FirstName,
            LastName=command.LastName,
            CreatedBy= "SOM13980",
            FunctionId=functionId,
            CountryId=countryId,
            BusinessProfileId=businessProfileId,
            Function=function,
            Country=country,
            BusinessProfile=businessProfile,
            Projects = [],
        });

        _mockAccessRequestRepository.Setup(x => x.AddAsync(It.IsAny<AccessRequest>()))
            .ReturnsAsync(expectedAccessRequest);

        var expectedDto = new AccessRequestDto { Id = expectedAccessRequest.Id };
        _mockMapper.Setup(x => x.Map<AccessRequestDto>(It.IsAny<AccessRequest>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result.AccessRequest);
        Assert.Equal("MSG.AccessRequestSubmitted", result.Message);

        // Vérifier que l'AccessRequest a été créé avec les bons IDs et entités
        _mockAccessRequestRepository.Verify(x => x.AddAsync(It.Is<AccessRequest>(ar =>
            ar.FunctionId == functionId &&
            ar.CountryId == countryId &&
            ar.BusinessProfileId == businessProfileId &&
            ar.Function == function &&
            ar.Country == country &&
            ar.BusinessProfile == businessProfile)), Times.Once);

        // Vérifier que les services de référence ont été appelés
        _mockReferenceService.Verify(x => x.GetFunctionByIdAsync(functionId, It.IsAny<CancellationToken>()), Times.Once);
        _mockReferenceService.Verify(x => x.GetBusinessProfileByIdAsync(businessProfileId, It.IsAny<CancellationToken>()), Times.Once);
        _mockReferenceService.Verify(x => x.GetCountryByIdAsync(countryId, It.IsAny<CancellationToken>()), Times.Once);
    }
}