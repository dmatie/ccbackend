namespace Afdb.ClientConnection.Application.DTOs;

public sealed record AccessRequestDocumentDto
{
    public Guid Id { get; init; }
    public Guid AccessRequestId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string DocumentUrl { get; init; } = string.Empty;
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
