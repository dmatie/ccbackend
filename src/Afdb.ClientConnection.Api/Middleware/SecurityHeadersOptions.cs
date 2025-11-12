namespace Afdb.ClientConnection.Api.Middleware;

public class SecurityHeadersOptions
{
    public const string SectionName = "SecurityHeaders";

    public bool EnableHsts { get; set; } = true;
    public bool EnableCsp { get; set; } = true;
    public bool EnableFrameOptions { get; set; } = true;
    public bool EnableReferrerPolicy { get; set; } = true;
    public bool EnablePermissionsPolicy { get; set; } = true;
    public bool EnableCacheControl { get; set; } = true;

    public string? Csp { get; set; }
    public string? ReferrerPolicy { get; set; }
    public string? PermissionsPolicy { get; set; }
    public string? FrameOptions { get; set; }
}
