namespace Afdb.ClientConnection.Infrastructure.Settings;

public sealed class FrontEndUrlsSettings
{
    public const string SectionName= "FrontEndUrls";

    public string BaseUrl { get; init; } = default!;
    public string UserAccessRequest { get; init; } = default!;
    public string UserAccessRequestAmend { get; init; } = default!;
    public string UserAccessRequestComplete { get; init; } = default!;
    public string UserClaim { get; init; } = default!;
    public string AdminClaim { get; init; } = default!;
    public string UserDisbursement { get; init; } = default!;
    public string AdminDisbursement { get; init; } = default!;
    public string UserOtherDocument { get; init; } = default!;
    public string AdminOtherDocument { get; init; } = default!;
}
