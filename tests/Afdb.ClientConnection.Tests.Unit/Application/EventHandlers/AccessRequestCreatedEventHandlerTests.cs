using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.EventHandlers;
using Afdb.ClientConnection.Domain.Events;
using Moq;

namespace Afdb.ClientConnection.Tests.Unit.Application.EventHandlers;

public class AccessRequestCreatedEventHandlerTests
{
    private readonly Mock<IServiceBusService> _mockServiceBusService;
    private readonly AccessRequestCreatedEventHandler _handler;

    public AccessRequestCreatedEventHandlerTests()
    {
        _mockServiceBusService = new Mock<IServiceBusService>();
        _handler = new AccessRequestCreatedEventHandler(_mockServiceBusService.Object);
    }

    [Fact]
    public async Task Handle_WithValidEvent_SendsMessageToServiceBus()
    {
        // Arrange
        var accessRequestEvent = new AccessRequestCreatedEvent(
            Guid.NewGuid(),
            "test@example.com",
            "John",
            "Doe",
            "ADB Desk Office", // Function
            "Executing Agency", // BusinessProfile
            "Algeria", // Country
            "Loan", // FinancingType
            "Pending", // Status
            new string[] { "approvers@example.com" });

        // Act
        await _handler.Handle(accessRequestEvent, CancellationToken.None);

        // Assert
        _mockServiceBusService.Verify(x => x.SendAccessRequestCreatedAsync(
            accessRequestEvent.AccessRequestId,
            accessRequestEvent.Email,
            accessRequestEvent.FirstName,
            accessRequestEvent.LastName,
            accessRequestEvent.Function,
            accessRequestEvent.BusinessProfile,
            accessRequestEvent.Country,
            accessRequestEvent.FinancingType,
            accessRequestEvent.Status,
            accessRequestEvent.ApproversEmail,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNullValues_SendsMessageToServiceBus()
    {
        // Arrange
        var accessRequestEvent = new AccessRequestCreatedEvent(
            Guid.NewGuid(),
            "test@example.com",
            "John",
            "Doe",
            null, // Function
            null, // BusinessProfile
            null, // Country
            null, // FinancingType
            "Pending", // Status
            new string[] { "approvers@example.com" });

        // Act
        await _handler.Handle(accessRequestEvent, CancellationToken.None);

        // Assert
        _mockServiceBusService.Verify(x => x.SendAccessRequestCreatedAsync(
            accessRequestEvent.AccessRequestId,
            accessRequestEvent.Email,
            accessRequestEvent.FirstName,
            accessRequestEvent.LastName,
            null, // Function
            null, // BusinessProfile
            null, // Country
            null, // FinancingType
            accessRequestEvent.Status,
            accessRequestEvent.ApproversEmail,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyApproversEmail_SendsMessageToServiceBus()
    {
        // Arrange
        var accessRequestEvent = new AccessRequestCreatedEvent(
            Guid.NewGuid(),
            "test@example.com",
            "John",
            "Doe",
            "Project Coordinator", // Function
            "Borrower", // BusinessProfile
            "Angola", // Country
            "Grant", // FinancingType
            "Pending", // Status
            Array.Empty<string>()); // Empty approvers

        // Act
        await _handler.Handle(accessRequestEvent, CancellationToken.None);

        // Assert
        _mockServiceBusService.Verify(x => x.SendAccessRequestCreatedAsync(
            accessRequestEvent.AccessRequestId,
            accessRequestEvent.Email,
            accessRequestEvent.FirstName,
            accessRequestEvent.LastName,
            accessRequestEvent.Function,
            accessRequestEvent.BusinessProfile,
            accessRequestEvent.Country,
            accessRequestEvent.FinancingType,
            accessRequestEvent.Status,
            accessRequestEvent.ApproversEmail,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithPartialNullValues_SendsMessageToServiceBus()
    {
        // Arrange
        var accessRequestEvent = new AccessRequestCreatedEvent(
            Guid.NewGuid(),
            "test@example.com",
            "John",
            "Doe",
            "ADB Desk Office", // Function
            null, // BusinessProfile
            "Morocco", // Country
            "Equity", // FinancingType
            "Pending", // Status
            new string[] { "approvers@example.com" });

        // Act
        await _handler.Handle(accessRequestEvent, CancellationToken.None);

        // Assert
        _mockServiceBusService.Verify(x => x.SendAccessRequestCreatedAsync(
            accessRequestEvent.AccessRequestId,
            accessRequestEvent.Email,
            accessRequestEvent.FirstName,
            accessRequestEvent.LastName,
            "ADB Desk Office", // Function
            null, // BusinessProfile
            "Morocco", // Country
            "Equity", // FinancingType
            accessRequestEvent.Status,
            accessRequestEvent.ApproversEmail,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithAllFinancingTypes_SendsMessageToServiceBus()
    {
        // Arrange
        var financingTypes = new[] { "Loan", "Grant", "Equity", "Guarantee", "Technical Assistance" };
        
        foreach (var financingType in financingTypes)
        {
            var accessRequestEvent = new AccessRequestCreatedEvent(
                Guid.NewGuid(),
                $"test-{financingType.ToLower()}@example.com",
                "John",
                "Doe",
                "ADB Desk Office", // Function
                "Executing Agency", // BusinessProfile
                "Algeria", // Country
                financingType, // FinancingType
                "Pending", // Status
                new string[] { "approvers@example.com" });

            // Act
            await _handler.Handle(accessRequestEvent, CancellationToken.None);

            // Assert
            _mockServiceBusService.Verify(x => x.SendAccessRequestCreatedAsync(
                accessRequestEvent.AccessRequestId,
                accessRequestEvent.Email,
                accessRequestEvent.FirstName,
                accessRequestEvent.LastName,
                accessRequestEvent.Function,
                accessRequestEvent.BusinessProfile,
                accessRequestEvent.Country,
                financingType, // Verify specific financing type
                accessRequestEvent.Status,
                accessRequestEvent.ApproversEmail,
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}