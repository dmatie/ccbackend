namespace Afdb.ClientConnection.Infrastructure.Settings;

public sealed class FrontEndUrlsSettings
{
    public const string SectionName= "FrontEndUrls";

    public string BaseUrl { get; init; } = default!;
    public string UserAccessRequest { get; init; } = default!;
    public string UserClaim { get; init; } = default!;
    public string AdminClaim { get; init; } = default!;
    public string UserDisbursement { get; init; } = default!;
    public string AdminDisbursement { get; init; } = default!;
}
