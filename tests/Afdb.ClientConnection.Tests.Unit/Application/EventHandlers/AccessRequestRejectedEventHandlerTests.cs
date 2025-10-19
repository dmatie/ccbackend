using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.EventHandlers;
using Afdb.ClientConnection.Domain.Events;
using Moq;

namespace Afdb.ClientConnection.Tests.Unit.Application.EventHandlers;

public class AccessRequestRejectedEventHandlerTests
{
    private readonly Mock<IServiceBusService> _mockServiceBusService;
    private readonly AccessRequestRejectedEventHandler _handler;

    public AccessRequestRejectedEventHandlerTests()
    {
        _mockServiceBusService = new Mock<IServiceBusService>();
        _handler = new AccessRequestRejectedEventHandler(_mockServiceBusService.Object);
    }

    [Fact]
    public async Task Handle_WithValidEvent_SendsMessageToServiceBus()
    {
        // Arrange
        var rejectedEvent = new AccessRequestRejectedEvent(
            Guid.NewGuid(),
            "rejected@example.com",
            "Bob",
            "Johnson",
            "Insufficient documentation provided");

        // Act
        await _handler.Handle(rejectedEvent, CancellationToken.None);

        // Assert
        _mockServiceBusService.Verify(x => x.SendAccessRequestRejectedAsync(
            rejectedEvent.AccessRequestId,
            rejectedEvent.Email,
            rejectedEvent.FirstName,
            rejectedEvent.LastName,
            rejectedEvent.RejectionReason,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}