namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IServiceBusService
{
    Task SendAccessRequestCreatedAsync(Guid accessRequestId, string email, string firstName,
           string lastName, string? function, string? businessProfile, string? country,
           string? financingType, string status, string[] approversEmail, 
           string documentFileName, string registrationCode,
           CancellationToken cancellationToken = default);

    Task SendAccessRequestApprovedAsync(Guid accessRequestId, string email, string firstName,
        string lastName, string approverEmail, CancellationToken cancellationToken = default);

    Task SendAccessRequestRejectedAsync(Guid accessRequestId, string email, string firstName,
        string lastName, string rejectionReason, CancellationToken cancellationToken = default);

    Task SendNotificationAsync<T>(T message, string queueName, CancellationToken cancellationToken = default)
        where T : class;
}