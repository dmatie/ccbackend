using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class AccessRequestCreatedEventHandler : INotificationHandler<AccessRequestCreatedEvent>
{
    private readonly IServiceBusService _serviceBusService;
    private readonly IAccessRequestDocumentService _accessRequestDocumentService;
    private readonly INotificationService _notificationService;

    public AccessRequestCreatedEventHandler(
        IServiceBusService serviceBusService,
        IAccessRequestDocumentService accessRequestDocumentService,
        INotificationService notificationService)
    {
        _serviceBusService = serviceBusService;
        _accessRequestDocumentService = accessRequestDocumentService;
        _notificationService = notificationService;
    }

    public async Task Handle(AccessRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        var (frenchPdf, idFr, englishPdf, idEn) = await _accessRequestDocumentService.GenerateAndUploadAuthorizationFormsAsync(
            notification.RegistrationCode,
            notification.FirstName,
            notification.LastName,
            notification.Email,
            notification.Function?? "N/A",
            notification.BusinessProfile?? "N/A",
            notification.Projects,
            cancellationToken);

        var notificationData = new Dictionary<string, object>
        {
            ["code"] = notification.RegistrationCode,
            ["firstName"] = notification.FirstName,
            ["lastName"] = notification.LastName,
            ["email"] = notification.Email,
            ["function"] = notification.Function ?? "N/A",
            ["businessProfile"] = notification.BusinessProfile ?? "N/A",
            ["country"] = notification.Country ?? "N/A",
            ["financingType"] = notification.FinancingType ?? "N/A",
            ["status"] = notification.Status
        };

        var attachments = new[]
        {
            new EmailAttachment
            {
                FileName = $"Formulaire_Autorisation_FR_{notification.RegistrationCode}.pdf",
                FileUrl = frenchPdf,
                FileIdentifier = idFr,
                ContentType = "application/pdf"
            },
            new EmailAttachment
            {
                FileName = $"Authorization_Form_EN_{notification.RegistrationCode}.pdf",
                FileUrl = englishPdf,
                FileIdentifier = idEn,
                ContentType = "application/pdf"
            }
        };

        var notificationRequest = new NotificationRequest
        {
            EventType = NotificationEventType.AccessRequestCreated,
            Recipient = notification.Email,
            RecipientName = $"{notification.FirstName} {notification.LastName}",
            Data = NotificationRequest.ConvertDictionaryToArray(notificationData),
            Attachments = attachments,
            Language = "en"
        };

        await _notificationService.SendNotificationAsync(notificationRequest, cancellationToken);
    }
}