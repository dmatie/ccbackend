namespace Afdb.ClientConnection.Infrastructure.Settings;

public sealed class SharePointSettings
{
    public const string SectionName = "SharePoint";

    public string SiteId { get; set; } = string.Empty;
    public string DriveId { get; set; } = string.Empty;
    public string ListId { get; set; } = string.Empty;
    public bool UseSharePointStorage { get; set; } = true;
}
