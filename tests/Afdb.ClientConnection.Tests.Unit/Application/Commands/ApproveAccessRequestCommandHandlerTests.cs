using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace Afdb.ClientConnection.Tests.Unit.Application.Commands;

public class ApproveAccessRequestCommandHandlerTests
{
    private readonly Mock<IAccessRequestRepository> _mockAccessRequestRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IGraphService> _mockGraphService;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<IAuditService> _mockAuditService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ApproveAccessRequestCommandHandler _handler;
    private readonly Mock<ILogger<ApproveAccessRequestCommandHandler>> _mockLogger;

    public ApproveAccessRequestCommandHandlerTests()
    {
        _mockAccessRequestRepository = new Mock<IAccessRequestRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockGraphService = new Mock<IGraphService>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockAuditService = new Mock<IAuditService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<ApproveAccessRequestCommandHandler>>();

        _handler = new ApproveAccessRequestCommandHandler(
            _mockAccessRequestRepository.Object,
            _mockUserRepository.Object,
            _mockCurrentUserService.Object,
            _mockAuditService.Object,
            _mockMapper.Object,
            _mockGraphService.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ApprovesAccessRequest()
    {
        // Arrange
        var command = new ApproveAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            Comments = "Approved for access"
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email= "test@example.com",
            FirstName= "John",
            LastName= "Doe",
            CreatedBy="System",
            //Projects = []

        });

        var currentUser = new User(
            "admin@example.com", "Admin", "User", UserRole.Admin,
            "admin-entra-id", "system");

        // ---- Mocks ----
        _mockAccessRequestRepository
            .Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync(accessRequest);

        // Important : exécuter réellement la fonction passée à ExecuteInTransactionAsync
        _mockAccessRequestRepository
            .Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
            .Returns<Func<Task>>(f => f());

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);
        _mockCurrentUserService.Setup(x => x.Email).Returns("admin@example.com");
        _mockCurrentUserService.Setup(x => x.UserId).Returns("admin-user-id");

        _mockUserRepository
            .Setup(x => x.GetByEmailAsync("admin@example.com"))
            .ReturnsAsync(currentUser);

        var expectedDto = new AccessRequestDto { Id = accessRequest.Id };
        _mockMapper
            .Setup(x => x.Map<AccessRequestDto>(It.IsAny<AccessRequest>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result.AccessRequest);
        Assert.Contains("MSG.AccessRequest.Approved", result.Message);

        // Vérifie que les opérations prévues ont bien eu lieu
        _mockAccessRequestRepository.Verify(
            x => x.UpdateAsync(It.IsAny<AccessRequest>()),
            Times.AtLeastOnce);

        _mockUserRepository.Verify(
            x => x.AddAsync(It.IsAny<User>()),
            Times.Once);

        _mockAuditService.Verify(
            x => x.LogAsync(
                nameof(AccessRequest),
                It.IsAny<Guid>(),
                "Approve",
                null,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenAccessRequestNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ApproveAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            Comments = "Approved"
        };

        _mockAccessRequestRepository.Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync((AccessRequest?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains(command.AccessRequestId.ToString(), exception.Message);
    }

    [Fact]
    public async Task Handle_WhenUserNotAuthorized_ThrowsForbiddenAccessException()
    {
        // Arrange
        var command = new ApproveAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            Comments = "Approved"
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedBy = "System",

        });

        _mockAccessRequestRepository.Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync(accessRequest);

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(false);
        _mockCurrentUserService.Setup(x => x.IsInRole("DO")).Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ForbiddenAccessException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("ERR.General.NotAuthorize", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ApproveAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            Comments = "Approved"
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedBy = "System",

        });

        _mockAccessRequestRepository.Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync(accessRequest);

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);
        _mockCurrentUserService.Setup(x => x.Email).Returns("admin@example.com");

        _mockUserRepository.Setup(x => x.GetByEmailAsync("admin@example.com"))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("ERR.General.UserNotExist", exception.Message);
    }
}
