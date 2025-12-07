namespace Afdb.ClientConnection.Application.DTOs;

public sealed class AccessRequestDocumentDto
{
    public Guid Id { get; set; }
    public Guid AccessRequestId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string DocumentUrl { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
