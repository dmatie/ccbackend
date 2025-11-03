using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.DTOs;

public sealed record CountryAdminDto
{
    public Guid CountryId { get; init; }
    public Guid UserId { get; init; }
    public bool IsActive { get; init; }
    public string? UserEmail { get; init; } = string.Empty;
    public string? UserFirstName { get; init; } = string.Empty;
    public string? UserLastName { get; init; } = string.Empty;
    public string? UserFullName { get; init; } = string.Empty;
    public string? CountryName { get; init; } = string.Empty;
    public string? CountryNameFr { get; init; } = string.Empty;
    public string? CounrtyCode { get; init; } = string.Empty;
}
