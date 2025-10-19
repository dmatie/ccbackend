using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.EventHandlers;
using Afdb.ClientConnection.Domain.Events;
using Moq;

namespace Afdb.ClientConnection.Tests.Unit.Application.EventHandlers;

public class AccessRequestApprovedEventHandlerTests
{
    private readonly Mock<IServiceBusService> _mockServiceBusService;
    private readonly AccessRequestApprovedEventHandler _handler;

    public AccessRequestApprovedEventHandlerTests()
    {
        _mockServiceBusService = new Mock<IServiceBusService>();
        _handler = new AccessRequestApprovedEventHandler(_mockServiceBusService.Object);
    }

    [Fact]
    public async Task Handle_WithValidEvent_SendsMessageToServiceBus()
    {
        // Arrange
        var approvedEvent = new AccessRequestApprovedEvent(
            Guid.NewGuid(),
            "approved@example.com",
            "Jane",
            "Smith");

        // Act
        await _handler.Handle(approvedEvent, CancellationToken.None);

        // Assert
        _mockServiceBusService.Verify(x => x.SendAccessRequestApprovedAsync(
            approvedEvent.AccessRequestId,
            approvedEvent.Email,
            approvedEvent.FirstName,
            approvedEvent.LastName,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
