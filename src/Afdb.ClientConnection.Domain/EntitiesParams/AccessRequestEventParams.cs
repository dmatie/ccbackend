using Afdb.ClientConnection.Domain.Events;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record AccessRequestEventParams
{
    public Guid AccessRequestId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? Function { get; init; }
    public string? FunctionFr { get; init; }
    public string? BusinessProfile { get; init; }
    public string? Country { get; init; }
    public string? FinancingType { get; init; }
    public string Status { get; init; } = string.Empty;
    public string[] ApproversEmail { get; init; } = Array.Empty<string>();
    public string RegistrationCode { get; init; } = string.Empty;
    public string DocumentFileName { get; init; } = string.Empty;
    public SelectedProjectCreatedEvent[] Projects { get; init; } = [];
}
