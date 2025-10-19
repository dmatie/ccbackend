namespace Afdb.ClientConnection.Application.DTOs;

public sealed record ProjectDto
{
    public string SapCode { get; init; } = default!;
    public string ProjectName { get; init; } = default!;
    public string CountryCode { get; init; }= default!;
    public string ManagingCountryCode { get; init; }=default!;
}
