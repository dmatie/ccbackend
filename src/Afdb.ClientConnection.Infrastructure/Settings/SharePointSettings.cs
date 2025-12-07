namespace Afdb.ClientConnection.Infrastructure.Settings;

public sealed class SharePointSettings
{
    public const string SectionName = "SharePoint";

    public string SiteId { get; set; } = string.Empty;
    public string DisbursementDriveId { get; set; } = string.Empty;
    public string DisbursementListId { get; set; } = string.Empty;
    public bool UseSharePointStorage { get; set; } = true;
    public string AccessRequestDriveId { get; set; } = string.Empty;
    public string AccessRequestListId { get; set; } = string.Empty;

}
