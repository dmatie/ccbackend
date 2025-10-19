using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class AuditLog : BaseEntity
{
    public string EntityName { get; private set; }
    public Guid EntityId { get; private set; }
    public string Action { get; private set; } // Create, Update, Approve, Reject, etc.
    public string? OldValues { get; private set; } // JSON
    public string? NewValues { get; private set; } // JSON
    public string UserId { get; private set; }
    public string UserName { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public DateTime Timestamp { get; private set; }

    private AuditLog() { } // For EF Core

    public AuditLog(string entityName, Guid entityId, string action, string userId, string userName,
        string? oldValues = null, string? newValues = null, string? ipAddress = null, string? userAgent = null)
    {
        if (string.IsNullOrWhiteSpace(entityName))
            throw new ArgumentException("Entity name cannot be empty", nameof(entityName));

        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action cannot be empty", nameof(action));

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty", nameof(userId));

        EntityName = entityName;
        EntityId = entityId;
        Action = action;
        OldValues = oldValues;
        NewValues = newValues;
        UserId = userId;
        UserName = userName;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Timestamp = DateTime.UtcNow;
        CreatedBy = userId;
    }
}
