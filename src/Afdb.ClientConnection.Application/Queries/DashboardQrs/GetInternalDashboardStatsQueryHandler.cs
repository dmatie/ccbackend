using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.Queries.DashboardQrs;

public sealed class GetInternalDashboardStatsQueryHandler : IRequestHandler<GetInternalDashboardStatsQuery, InternalDashboardStatsDto>
{
    private readonly IAccessRequestRepository _accessRequestRepository;
    private readonly IClaimRepository _claimRepository;
    private readonly IDisbursementRepository _disbursementRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetInternalDashboardStatsQueryHandler> _logger;

    public GetInternalDashboardStatsQueryHandler(
        IAccessRequestRepository accessRequestRepository,
        IClaimRepository claimRepository,
        IDisbursementRepository disbursementRepository,
        IUserRepository userRepository,
        ILogger<GetInternalDashboardStatsQueryHandler> logger)
    {
        _accessRequestRepository = accessRequestRepository;
        _claimRepository = claimRepository;
        _disbursementRepository = disbursementRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<InternalDashboardStatsDto> Handle(GetInternalDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching internal dashboard statistics");

        var pendingAccessRequests = await _accessRequestRepository.CountByStatusAsync(RequestStatus.Pending, cancellationToken);
        var pendingClaims = await _claimRepository.CountByStatusAsync(ClaimStatus.Submitted, cancellationToken);
        var pendingDisbursements = await _disbursementRepository.CountByStatusAsync(DisbursementStatus.Submitted, cancellationToken);
        var totalUsers = await _userRepository.CountByRoleAsync(UserRole.Internal, cancellationToken);

        _logger.LogInformation(
            "Internal dashboard stats - PendingAccessRequests: {PendingAccessRequests}, PendingClaims: {PendingClaims}, PendingDisbursements: {PendingDisbursements}, TotalUsers: {TotalUsers}",
            pendingAccessRequests,
            pendingClaims,
            pendingDisbursements,
            totalUsers);

        return new InternalDashboardStatsDto
        {
            PendingAccessRequests = pendingAccessRequests,
            PendingClaims = pendingClaims,
            PendingDisbursements = pendingDisbursements,
            TotalUsers = totalUsers
        };
    }
}
