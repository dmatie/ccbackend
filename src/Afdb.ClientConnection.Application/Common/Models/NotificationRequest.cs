using Afdb.ClientConnection.Application.Common.Enums;

namespace Afdb.ClientConnection.Application.Common.Models;

public sealed record NotificationRequest
{
    public NotificationEventType EventType { get; init; }

    public string Recipient { get; init; } = string.Empty;

    public string? RecipientName { get; init; }= string.Empty;

    public string[]? AdditionalRecipients { get; init; } = [];

    public string[]? CcRecipients { get; init; }= [];

    public NotificationDataItem [] Data { get; init; } = [];

    public string Language { get; init; } = "en";
    public EmailAttachment[]? Attachments { get; init; } = [];

    public static NotificationDataItem [] ConvertDictionaryToArray(Dictionary<string, object> dict)
    {
        return [.. dict.Select(kvp => new NotificationDataItem { Key = kvp.Key.ToLower(), Value = kvp.Value.ToString() })];
    }
}

public sealed record NotificationDataItem
{
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}

public sealed record EmailAttachment
{
    public string FileName { get; init; } = string.Empty;
    public string FileUrl { get; init; } = string.Empty;
    public string FileIdentifier { get; set; } = string.Empty;
    public string ContentType { get; init; } = "application/pdf";
}
