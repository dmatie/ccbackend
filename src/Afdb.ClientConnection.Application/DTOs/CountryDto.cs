
namespace Afdb.ClientConnection.Application.DTOs;

public sealed record CountryDto
{
    public Guid Id { get; init;}
    public string Name { get; init; } = string.Empty;
    public string NameFr { get;  init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public bool IsActive { get; init; } 
}
