using Microsoft.Extensions.Options;

namespace Afdb.ClientConnection.Api.Middleware;

public class SecurityHeadersMiddleware(RequestDelegate next, IOptions<SecurityHeadersOptions> options)
{
    private readonly RequestDelegate _next = next;
    private readonly SecurityHeadersOptions _options = options.Value;

    public async Task InvokeAsync(HttpContext context)
    {
        var headers = context.Response.Headers;

        // ---- TRANSPORT ----
        if (_options.EnableHsts)
        {
            headers.StrictTransportSecurity = "max-age=63072000; includeSubDomains; preload";
        }

        headers.XContentTypeOptions = "nosniff";

        // ---- PROTECTION INTERFACE ----
        if (_options.EnableFrameOptions)
            headers.XFrameOptions = _options.FrameOptions ?? "DENY";

        if (_options.EnableReferrerPolicy)
            headers["Referrer-Policy"] = _options.ReferrerPolicy ?? "strict-origin-when-cross-origin";

        // ---- PERMISSIONS NAVIGATEUR ----
        if (_options.EnablePermissionsPolicy)
        {
            headers["Permissions-Policy"] = _options.PermissionsPolicy ?? "geolocation=(), microphone=(), camera=()";
            headers["Cross-Origin-Resource-Policy"] = "same-origin";
            headers["Cross-Origin-Opener-Policy"] = "same-origin";
            headers["Cross-Origin-Embedder-Policy"] = "require-corp";
        }

        // ---- SÉCURITÉ DU CONTENU ----
        if (_options.EnableCsp)
        {
            headers.ContentSecurityPolicy =
                _options.Csp ??
                "default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline';"+
                " img-src 'self' data:; font-src 'self'; object-src 'none'; base-uri 'self'; form-action 'self';";
        }

        // ---- CACHE ----
        if (_options.EnableCacheControl)
        {
            headers.CacheControl = "no-store, no-cache, must-revalidate";
            headers.Pragma = "no-cache";
            headers.Expires = "0";
        }

        await _next(context);
    }
}

public static class SecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SecurityHeadersMiddleware>();
    }
}