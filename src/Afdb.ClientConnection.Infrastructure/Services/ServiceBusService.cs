using Afdb.ClientConnection.Application.Common.Interfaces;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class ServiceBusService : IServiceBusService, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _accessRequestSender;
    private readonly ServiceBusSender _accessRequestResponseSender;
    private readonly ILogger<ServiceBusService> _logger;

    public ServiceBusService(ServiceBusClient client, IConfiguration configuration, ILogger<ServiceBusService> logger)
    {
        _client = client;
        _logger = logger;

        var accessRequestQueue = configuration["ServiceBus:AccessRequestQueue"] ?? "access-requests";
        var accessRequestResponseQueue = configuration["ServiceBus:AccessRequestResponseQueue"] ?? "notifications";

        _accessRequestSender = _client.CreateSender(accessRequestQueue);
        _accessRequestResponseSender = _client.CreateSender(accessRequestResponseQueue);
    }

    public async Task SendAccessRequestCreatedAsync(Guid accessRequestId, string email, string firstName,
    string lastName, string? function, string? businessProfile, string? country,
    string? financingType, string status, string[] approversEmail,
    CancellationToken cancellationToken = default)
    {
        var message = new
        {
            EventType = "AccessRequestCreated",
            AccessRequestId = accessRequestId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Function = function,
            BusinessProfile = businessProfile,
            Country = country,
            FinancingType = financingType,
            Status = status,
            ApproversEmail = approversEmail,
            CreatedAt = DateTime.UtcNow
        };

        await SendMessageAsync(_accessRequestSender, message, "AccessRequestCreated", cancellationToken);
    }

    public async Task SendAccessRequestApprovedAsync(Guid accessRequestId, string email, string firstName,
        string lastName, CancellationToken cancellationToken = default)
    {
        var message = new
        {
            EventType = "AccessRequestApproved",
            AccessRequestId = accessRequestId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            ApprovedAt = DateTime.UtcNow
        };

        await SendMessageAsync(_accessRequestResponseSender, message, "AccessRequestApproved", cancellationToken);
    }

    public async Task SendAccessRequestRejectedAsync(Guid accessRequestId, string email, string firstName,
        string lastName, string rejectionReason, CancellationToken cancellationToken = default)
    {
        var message = new
        {
            EventType = "AccessRequestRejected",
            AccessRequestId = accessRequestId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            RejectionReason = rejectionReason,
            RejectedAt = DateTime.UtcNow
        };

        await SendMessageAsync(_accessRequestResponseSender, message, "AccessRequestRejected", cancellationToken);
    }

    public async Task SendNotificationAsync<T>(T message, string queueName, CancellationToken cancellationToken = default)
        where T : class
    {
        var sender = _client.CreateSender(queueName);
        try
        {
            await SendMessageAsync(sender, message, typeof(T).Name, cancellationToken);
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }

    private async Task SendMessageAsync<T>(ServiceBusSender sender, T message, string subject, CancellationToken cancellationToken)
    {
        try
        {
            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var serviceBusMessage = new ServiceBusMessage(json)
            {
                ContentType = "application/json",
                Subject = subject,
                MessageId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(serviceBusMessage, cancellationToken);

            _logger.LogInformation("Message sent to Service Bus: {Subject} with ID: {MessageId}",
                subject, serviceBusMessage.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send message to Service Bus: {Subject}", subject);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _accessRequestSender.DisposeAsync();
        await _accessRequestResponseSender.DisposeAsync();
        await _client.DisposeAsync();
    }
}
