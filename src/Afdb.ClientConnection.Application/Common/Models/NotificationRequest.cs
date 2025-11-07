using Afdb.ClientConnection.Application.Common.Enums;

namespace Afdb.ClientConnection.Application.Common.Models;

public sealed record NotificationRequest
{
    public NotificationEventType EventType { get; init; }

    public string Recipient { get; init; } = string.Empty;

    public string? RecipientName { get; init; }

    public string[]? AdditionalRecipients { get; init; }

    public string[]? CcRecipients { get; init; }

    public Dictionary<string, object> Data { get; init; } = new();

    public string Language { get; init; } = "en";
}
