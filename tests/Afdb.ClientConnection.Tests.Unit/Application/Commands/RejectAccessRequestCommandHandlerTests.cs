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

public class RejectAccessRequestCommandHandlerTests
{
    private readonly Mock<IAccessRequestRepository> _mockAccessRequestRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IServiceBusService> _mockServiceBusService;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<IAuditService> _mockAuditService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly RejectAccessRequestCommandHandler _handler;

    public RejectAccessRequestCommandHandlerTests()
    {
        _mockAccessRequestRepository = new Mock<IAccessRequestRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockServiceBusService = new Mock<IServiceBusService>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockAuditService = new Mock<IAuditService>();
        _mockMapper = new Mock<IMapper>();

        _handler = new RejectAccessRequestCommandHandler(
            _mockAccessRequestRepository.Object,
            _mockUserRepository.Object,
            _mockCurrentUserService.Object,
            _mockAuditService.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_RejectsAccessRequest()
    {
        // Arrange
        var command = new RejectAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            RejectionReason = "Insufficient documentation provided"
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName="Doe",
            CreatedBy="System"
        });

        var currentUser = new User(
            "admin@example.com", "Admin", "User", UserRole.DO,
            "admin-entra-id", "system");

        _mockAccessRequestRepository.Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync(accessRequest);

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(false);
        _mockCurrentUserService.Setup(x => x.IsInRole("DO")).Returns(true);
        _mockCurrentUserService.Setup(x => x.Email).Returns("admin@example.com");
        _mockCurrentUserService.Setup(x => x.UserId).Returns("admin-user-id");

        _mockUserRepository.Setup(x => x.GetByEmailAsync("admin@example.com"))
            .ReturnsAsync(currentUser);

        var expectedDto = new AccessRequestDto { Id = accessRequest.Id };
        _mockMapper.Setup(x => x.Map<AccessRequestDto>(It.IsAny<AccessRequest>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result.AccessRequest);
        Assert.Contains("MSG.AccessRequest.Rejected", result.Message);

        _mockAccessRequestRepository.Verify(x => x.UpdateAsync(It.IsAny<AccessRequest>()), Times.Once);
        _mockAuditService.Verify(x => x.LogAsync(
            nameof(AccessRequest), It.IsAny<Guid>(), "Reject",
            null, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenAccessRequestNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new RejectAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            RejectionReason = "Not found"
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
        var command = new RejectAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            RejectionReason = "Unauthorized rejection"
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedBy = "System"
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
        var command = new RejectAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            RejectionReason = "User not found"
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedBy = "System"
        });

        _mockAccessRequestRepository.Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync(accessRequest);

        _mockCurrentUserService.Setup(x => x.IsInRole("DO")).Returns(true);
        _mockCurrentUserService.Setup(x => x.Email).Returns("admin@example.com");

        _mockUserRepository.Setup(x => x.GetByEmailAsync("admin@example.com"))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("ERR.General.UserNotExist", exception.Message);
    }

    [Fact]
    public async Task Handle_WithAdminRole_CanRejectRequest()
    {
        // Arrange
        var command = new RejectAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            RejectionReason = "Admin rejection"
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedBy = "System"
        });

        var currentUser = new User(
            "admin@example.com", "Admin", "User", UserRole.Admin,
            "admin-entra-id", "system");

        _mockAccessRequestRepository.Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync(accessRequest);

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);
        _mockCurrentUserService.Setup(x => x.IsInRole("DO")).Returns(false);
        _mockCurrentUserService.Setup(x => x.Email).Returns("admin@example.com");
        _mockCurrentUserService.Setup(x => x.UserId).Returns("admin-user-id");

        _mockUserRepository.Setup(x => x.GetByEmailAsync("admin@example.com"))
            .ReturnsAsync(currentUser);

        var expectedDto = new AccessRequestDto { Id = accessRequest.Id };
        _mockMapper.Setup(x => x.Map<AccessRequestDto>(It.IsAny<AccessRequest>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result.AccessRequest);
        _mockAccessRequestRepository.Verify(x => x.UpdateAsync(It.IsAny<AccessRequest>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenAccessRequestAlreadyProcessed_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new RejectAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            RejectionReason = "Already processed"
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedBy = "System"
        });

        // Simuler une demande déjà traitée (approuvée)
        accessRequest.Approve(Guid.NewGuid(), "Already approved", "admin",true);

        var currentUser = new User(
            "admin@example.com", "Admin", "User", UserRole.Admin,
            "admin-entra-id", "system");

        _mockAccessRequestRepository.Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync(accessRequest);

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);
        _mockCurrentUserService.Setup(x => x.Email).Returns("admin@example.com");
        _mockCurrentUserService.Setup(x => x.UserId).Returns("admin-user-id");

        _mockUserRepository.Setup(x => x.GetByEmailAsync("admin@example.com"))
            .ReturnsAsync(currentUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("Only pending requests can be rejected", exception.Message);
    }

    [Fact]
    public async Task Handle_WithValidCommand_SendsRejectionEvent()
    {
        // Arrange
        var command = new RejectAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            RejectionReason = "Insufficient documentation"
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedBy = "System"
        });

        var currentUser = new User(
            "admin@example.com", "Admin", "User", UserRole.Admin,
            "admin-entra-id", "system");

        _mockAccessRequestRepository.Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync(accessRequest);

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);
        _mockCurrentUserService.Setup(x => x.Email).Returns("admin@example.com");
        _mockCurrentUserService.Setup(x => x.UserId).Returns("admin-user-id");

        _mockUserRepository.Setup(x => x.GetByEmailAsync("admin@example.com"))
            .ReturnsAsync(currentUser);

        var expectedDto = new AccessRequestDto { Id = accessRequest.Id };
        _mockMapper.Setup(x => x.Map<AccessRequestDto>(It.IsAny<AccessRequest>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result.AccessRequest);

        // Vérifier que la demande a été rejetée
        Assert.Equal(RequestStatus.Rejected, accessRequest.Status);
        Assert.Equal(command.RejectionReason, accessRequest.ProcessingComments);
        Assert.NotNull(accessRequest.ProcessedDate);

        _mockAccessRequestRepository.Verify(x => x.UpdateAsync(It.IsAny<AccessRequest>()), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Handle_WithEmptyRejectionReason_ThrowsArgumentException(string rejectionReason)
    {
        // Arrange
        var command = new RejectAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            RejectionReason = rejectionReason
        };

        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CreatedBy = "System"
        });

        var currentUser = new User(
            "admin@example.com", "Admin", "User", UserRole.Admin,
            "admin-entra-id", "system");

        _mockAccessRequestRepository.Setup(x => x.GetByIdAsync(command.AccessRequestId))
            .ReturnsAsync(accessRequest);

        _mockCurrentUserService.Setup(x => x.IsInRole("Admin")).Returns(true);
        _mockCurrentUserService.Setup(x => x.Email).Returns("admin@example.com");
        _mockCurrentUserService.Setup(x => x.UserId).Returns("admin-user-id");

        _mockUserRepository.Setup(x => x.GetByEmailAsync("admin@example.com"))
            .ReturnsAsync(currentUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("Rejection reason is required", exception.Message);
    }
}