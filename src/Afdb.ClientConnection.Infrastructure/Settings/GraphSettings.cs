namespace Afdb.ClientConnection.Infrastructure.Settings;

public sealed class GraphSettings
{
    public const string SectionName = "Graph";
    public string AddGroupMemberUrl { get; set; } = string.Empty;
    public string InviteRedirectUrl { get; set; } = string.Empty;
    public string[] MockAdminUsers { get; set; } = [];
    public string[] MockFifc3DOUsers { get; set; } = [];
    public string[] MockFifc3DAUsers { get; set; } = [];
    public bool UseMock { get; set; }
    public AppGroups AppGroups { get; set; } = new();
    public AppRoles AppRoles { get; set; } = new();
}

public sealed class AppGroups
{
    public string FifcAdmin { get; set; } = string.Empty;
    public string FifcDO { get; set; } = string.Empty;
    public string FifcDA { get; set; } = string.Empty;
    public string External { get; set; } = string.Empty;
}

public sealed class AppRoles
{
    public string Admin { get; set; } = string.Empty;
    public string DO { get; set; } = string.Empty;
    public string DA { get; set; } = string.Empty;
    public string External { get; set; } = string.Empty;
}
