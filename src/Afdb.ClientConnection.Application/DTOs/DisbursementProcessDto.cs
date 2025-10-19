using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementProcessDto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    public DisbursementStatus Status { get; init; }
    public Guid ProcessedByUserId { get; init; }
    public string ProcessedByUserName { get; init; } = string.Empty;
    public string ProcessedByUserEmail { get; init; } = string.Empty;
    public string Comment { get; init; } = string.Empty;
    public DateTime ProcessedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
