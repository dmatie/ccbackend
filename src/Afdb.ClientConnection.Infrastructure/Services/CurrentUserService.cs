using Afdb.ClientConnection.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

    public string Email => GetUserEmail(_httpContextAccessor.HttpContext?.User);

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    public bool IsAppAuthentification => _httpContextAccessor.HttpContext?.User?.FindFirstValue("appid") != null;

    public IEnumerable<string> Roles => _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)?.Select(c => c.Value) ?? Enumerable.Empty<string>();

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    }

    private static string GetUserEmail(ClaimsPrincipal? user)
    {
        if (user == null)
            return string.Empty;

        return user?.FindFirst(ClaimTypes.Email)?.Value
            ?? user?.FindFirst(ClaimTypes.Upn)?.Value
            ?? string.Empty;
    }

}