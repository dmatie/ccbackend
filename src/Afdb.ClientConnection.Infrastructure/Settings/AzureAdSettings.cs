namespace Afdb.ClientConnection.Infrastructure.Settings;

public sealed class AzureAdSettings
{
    public const string SectionName= "AzureAd";

    public string Instance { get; init; } = default!;
    public string Domain { get; init; } = default!;
    public string TenantId { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public string Audiance { get; init; } = default!;
}
