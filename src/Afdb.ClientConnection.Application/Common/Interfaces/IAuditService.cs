namespace Afdb.ClientConnection.Application.Common.Interfaces;
public interface IAuditService
{
    Task LogAsync(string entityName, Guid entityId, string action, string? oldValues = null,
        string? newValues = null, CancellationToken cancellationToken = default);
}