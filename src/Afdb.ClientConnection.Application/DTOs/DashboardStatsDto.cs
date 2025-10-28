namespace Afdb.ClientConnection.Application.DTOs;

public sealed record InternalDashboardStatsDto
{
    public int PendingAccessRequests { get; init; }
    public int PendingClaims { get; init; }
    public int PendingDisbursements { get; init; }
    public int TotalUsers { get; init; }
}

public sealed record ExternalDashboardStatsDto
{
    public int ActiveProjects { get; init; }
    public int ActiveDisbursementRequests { get; init; }
    public int PendingClaims { get; init; }
}
