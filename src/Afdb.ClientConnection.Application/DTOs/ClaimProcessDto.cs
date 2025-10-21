using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.DTOs;

public sealed record ClaimProcessDto
{
    public Guid Id { get; init; }
    public Guid ClaimId { get; init; }
    public Guid UserId { get; init; }
    public string UserFirstName { get; init; } = string.Empty;
    public string UserLastName { get; init; } = string.Empty;
    public string UserFullName { get; init; } = string.Empty;
    public ClaimStatus Status { get; init; }
    public string Comment { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
}
