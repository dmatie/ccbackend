using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.DTOs;

public sealed record ClaimDto
{
    public Guid Id { get; init; }

    public Guid ClaimTypeId { get; init; }
    public string ClaimTypeName { get; init; } = string.Empty;
    public string ClaimTypeNameFr { get; init; } = string.Empty;

    public Guid CountryId { get; init; }
    public string CountryName { get; init; } = string.Empty;

    public Guid UserId { get; init; }
    public string UserFirstName { get; init; } = string.Empty;
    public string UserLastName { get; init; } = string.Empty;
    public string UserFullName { get; init; } = string.Empty;
    public string UserEmail { get; init; } = string.Empty;

    public ClaimStatus Status { get; init; }
    public DateTime? ClosedAt { get; init; }
    public string Comment { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime? UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }

    public List<ClaimProcessDto> Processes { get; init; } = [];
}
