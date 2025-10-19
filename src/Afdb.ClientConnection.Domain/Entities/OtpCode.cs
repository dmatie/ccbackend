using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class OtpCode : AggregateRoot
{
    public string Email { get; private set; }
    public string Code { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    private OtpCode() { } // For EF Core
    public OtpCode(string email, string code, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty");
        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("Expiration time must be in the future");
        Email = email.ToLowerInvariant();
        Code = code;
        ExpiresAt = expiresAt;
    }
}
