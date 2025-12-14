using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.DTOs;

public sealed record OtherDocumentDto
{
    public Guid Id { get; init; }
    public Guid OtherDocumentTypeId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Year { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public OtherDocumentStatus Status { get; init; }
    public string SAPCode { get; init; } = string.Empty;
    public string LoanNumber { get; init; } = string.Empty;

    public OtherDocumentTypeDto? OtherDocumentType { get; init; }
    public UserDto? User { get; init; }
    public List<OtherDocumentFileDto> Files { get; init; } = [];

    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime? UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
}
