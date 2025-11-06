using Afdb.ClientConnection.Application.Common.Models;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default);
}
