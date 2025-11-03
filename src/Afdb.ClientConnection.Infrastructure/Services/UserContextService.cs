using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Infrastructure.Services;

public sealed class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string UserContextKey = "UserContext";

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserContext GetUserContext()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is not available");

        if (!httpContext.Items.ContainsKey(UserContextKey))
            throw new InvalidOperationException("UserContext not loaded. Ensure UserContextMiddleware is registered.");

        return (UserContext)httpContext.Items[UserContextKey]!;
    }

    public bool HasUserContext()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.Items.ContainsKey(UserContextKey) == true;
    }
}
