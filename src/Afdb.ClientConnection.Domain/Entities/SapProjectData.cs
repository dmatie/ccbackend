namespace Afdb.ClientConnection.Domain.Entities;

public sealed class SapProjectData
{
    public string CountryCode { get; set; } = default!;
    public string ProjectCode { get; set; } = default!;
    public string ProjectTitle { get; set; } = default!;
    public string projectDescription { get; set; } = default!;
}
