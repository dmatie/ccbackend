namespace Afdb.ClientConnection.Application.DTOs;

public sealed record AccessRequestProjectDto
{
    public Guid AccessRequestId { get; init; }
    public string SapCode { get; init; } = default!;
}
