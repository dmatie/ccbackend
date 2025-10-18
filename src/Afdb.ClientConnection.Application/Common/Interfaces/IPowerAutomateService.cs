using Afdb.ClientConnection.Domain.Events;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IPowerAutomateService
{
    Task NotifyClaimCreatedAsync(ClaimCreatedEvent claimCreatedEvent);
    Task NotifyClaimResponseAddedAsync(ClaimProcessAddedEvent claimResponseAddedEvent);
    Task NotifyOtpCreatedAsync(string customerEmail, string code);
}
