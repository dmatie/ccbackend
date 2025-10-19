namespace Afdb.ClientConnection.Domain.Entities;

public sealed class Token
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string? Scope { get; set; }

}
