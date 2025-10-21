namespace Afdb.ClientConnection.Application.DTOs;

public sealed record OpCodeDto
{
    public string Code { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
}
