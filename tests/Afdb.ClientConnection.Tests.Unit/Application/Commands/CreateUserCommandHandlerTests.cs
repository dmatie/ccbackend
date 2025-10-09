using Afdb.ClientConnection.Application.Commands.UserCmd;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using Moq;

namespace Afdb.ClientConnection.Tests.Unit.Application.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<IAuditService> _mockAuditService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockAuditService = new Mock<IAuditService>();
        _mockMapper = new Mock<IMapper>();

        _handler = new CreateUserCommandHandler(
            _mockUserRepository.Object,
            _mockCurrentUserService.Object,
            _mockAuditService.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesUser()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Role = UserRole.DO,
            EntraIdObjectId = "entra-123"
        };

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);
        _mockCurrentUserService.Setup(x => x.UserId).Returns("admin-user-id");

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        _mockUserRepository.Setup(x => x.GetByEntraIdObjectIdAsync(command.EntraIdObjectId))
            .ReturnsAsync((User?)null);

        var expectedUser = new User(
            command.Email, command.FirstName, command.LastName,
            command.Role, command.EntraIdObjectId, "admin-user-id");

        _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(expectedUser);

        var expectedDto = new UserDto { Id = expectedUser.Id };
        _mockMapper.Setup(x => x.Map<UserDto>(It.IsAny<User>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result.User);
        Assert.Contains("MSG.User.Created", result.Message);

        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        _mockAuditService.Verify(x => x.LogAsync(
            nameof(User), It.IsAny<Guid>(), "Create",
            null, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserNotAuthorized_ThrowsForbiddenAccessException()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "testForbien@example.com",
            FirstName = "Test",
            LastName = "Forbiden",
            Role = UserRole.DO
        };

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("ERR.General.NotAuthorize", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "existing@example.com",
            FirstName = "Existing",
            LastName = "Mail",
            Role = UserRole.DO
        };

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);

        var existingUser = new User(
            command.Email, "Existing", "User", UserRole.DA,
            "existing-entra", "system");

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("ERR.User.EmailAlreadyExists", exception.Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task Handle_ExternalUserWithoutEntraId_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "external@example.com",
            FirstName = "External",
            LastName = "User",
            Role = UserRole.ExternalUser,
            OrganizationName = "External Org"
            // EntraIdObjectId manquant
        };

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);
        _mockCurrentUserService.Setup(x => x.UserId).Returns("admin-user-id");

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("ERR.User.MandatoryEntraId", exception.Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task Handle_ExternalUserWithValidData_CreatesExternalUser()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "externalvalid@example.com",
            FirstName = "Valid external",
            LastName = "User",
            Role = UserRole.ExternalUser,
            EntraIdObjectId = "external-entra-123",
            OrganizationName = "External Organization"
        };

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);
        _mockCurrentUserService.Setup(x => x.UserId).Returns("admin-user-id");

        _mockUserRepository.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        _mockUserRepository.Setup(x => x.GetByEntraIdObjectIdAsync(command.EntraIdObjectId))
            .ReturnsAsync((User?)null);

        var expectedUser = User.CreateExternalUser(
            command.Email, command.FirstName, command.LastName,
            command.OrganizationName, command.EntraIdObjectId, "admin-user-id");

        _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(expectedUser);

        var expectedDto = new UserDto { Id = expectedUser.Id };
        _mockMapper.Setup(x => x.Map<UserDto>(It.IsAny<User>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result.User);
        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
    }
}