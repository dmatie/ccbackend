using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.Queries.DashboardQrs;

public sealed class GetExternalDashboardStatsQueryHandler : IRequestHandler<GetExternalDashboardStatsQuery, ExternalDashboardStatsDto>
{
    private readonly IAccessRequestRepository _accessRequestRepository;
    private readonly IClaimRepository _claimRepository;
    private readonly IDisbursementRepository _disbursementRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<GetExternalDashboardStatsQueryHandler> _logger;

    public GetExternalDashboardStatsQueryHandler(
        IAccessRequestRepository accessRequestRepository,
        IClaimRepository claimRepository,
        IDisbursementRepository disbursementRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        ILogger<GetExternalDashboardStatsQueryHandler> logger)
    {
        _accessRequestRepository = accessRequestRepository;
        _claimRepository = claimRepository;
        _disbursementRepository = disbursementRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<ExternalDashboardStatsDto> Handle(GetExternalDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching external dashboard statistics for user: {Email}", _currentUserService.Email);

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        var activeProjects = await _accessRequestRepository.CountProjectsByUserIdAsync(user.Email, cancellationToken);
        var activeDisbursementRequests = await _disbursementRepository
            .CountByUserIdAndStatusAsync(user.Id, DisbursementStatus.Submitted, cancellationToken);
        var pendingClaims = await _claimRepository.CountByUserIdAndStatusAsync(user.Id, ClaimStatus.Submitted, cancellationToken);

        _logger.LogInformation(
            "External dashboard stats for user {UserId} - ActiveProjects: {ActiveProjects}, " +
            "ActiveDisbursementRequests: {ActiveDisbursementRequests}, PendingClaims: {PendingClaims}",
            user.Id,
            activeProjects,
            activeDisbursementRequests,
            pendingClaims);

        return new ExternalDashboardStatsDto
        {
            ActiveProjects = activeProjects,
            ActiveDisbursementRequests = activeDisbursementRequests,
            PendingClaims = pendingClaims
        };
    }
}
