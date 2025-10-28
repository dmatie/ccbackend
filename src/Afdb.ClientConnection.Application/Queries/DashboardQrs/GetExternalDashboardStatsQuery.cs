using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DashboardQrs;

public sealed record GetExternalDashboardStatsQuery : IRequest<ExternalDashboardStatsDto>
{
}
