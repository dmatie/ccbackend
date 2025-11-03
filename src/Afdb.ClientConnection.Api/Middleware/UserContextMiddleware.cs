using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;

namespace Afdb.ClientConnection.Api.Middleware;

public sealed class UserContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserContextMiddleware> _logger;
    private const string UserContextKey = "UserContext";

    public UserContextMiddleware(
        RequestDelegate next,
        ILogger<UserContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        ICountryAdminRepository countryAdminRepository)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        try
        {
            var userIdString = currentUserService.UserId;

            if (string.IsNullOrEmpty(userIdString))
            {
                _logger.LogWarning("User authenticated but UserId is null");
                await _next(context);
                return;
            }

            if (!Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("Invalid UserId format: {UserId}", userIdString);
                await _next(context);
                return;
            }

            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("User not found in database: {UserId}", userId);
                await _next(context);
                return;
            }

            var countryIds = new List<Guid>();
            if (user.RequiresCountryAssignment)
            {
                var countryAdmins = await countryAdminRepository
                    .GetByUserIdAsync(userId, context.RequestAborted);

                if (countryAdmins != null)
                {
                    countryIds = countryAdmins
                        .Where(ca => ca.IsActive)
                        .Select(ca => ca.CountryId)
                        .ToList();
                }

                _logger.LogInformation(
                    "Loaded {CountryCount} countries for user {UserId} with role {Role}",
                    countryIds.Count,
                    userId,
                    user.Role);
            }

            var userContext = new UserContext
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role,
                CountryIds = countryIds
            };

            context.Items[UserContextKey] = userContext;

            _logger.LogDebug(
                "UserContext loaded: UserId={UserId}, Email={Email}, Role={Role}, Countries={CountryCount}",
                userContext.UserId,
                userContext.Email,
                userContext.Role,
                userContext.CountryIds.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading UserContext");
        }

        await _next(context);
    }
}

public static class UserContextMiddlewareExtensions
{
    public static IApplicationBuilder UseUserContext(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserContextMiddleware>();
    }
}
