using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Afdb.ClientConnection.Tests.Integration;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{

    public Mock<IGraphService> MockGraphService { get; private set; } = null!;
    public Mock<IServiceBusService> MockServiceBusService { get; private set; } = null!;
    public Mock<IAuditService> MockAuditService { get; private set; } = null!;

    public Mock<IMediator> MockMediator { get; private set; } = null!;

    public string AdminUserEmail { get; } = "admintest@afdb.com";
    public string AdminUserFirstName { get; } = "Test";
    public string AdminUserLastName { get; } = "Admin";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.Test.json", optional: false);
        });

        builder.ConfigureServices(services =>
        {
            // Supprimer l’ancien DbContext (SqlServer normal)
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ClientConnectionDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Recréer le DbContext avec la chaîne de connexion de test
            var sp = services.BuildServiceProvider();
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DbConnectionString");

            services.AddDbContext<ClientConnectionDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Build final et reset DB
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ClientConnectionDbContext>();
            db.Database.EnsureDeleted();
            db.Database.Migrate();

            // Mock external services
            MockExternalServices(services);
            // Ajouter une authentification factice

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });
        });
    }


    protected virtual void MockExternalServices(IServiceCollection services)
    {
        // Remove real services
        var graphDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IGraphService));
        if (graphDescriptor != null) services.Remove(graphDescriptor);

        var serviceBusDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IServiceBusService));
        if (serviceBusDescriptor != null) services.Remove(serviceBusDescriptor);

        var auditDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuditService));
        if (auditDescriptor != null) services.Remove(auditDescriptor);

        // Add mocked services
        MockGraphService = new Mock<IGraphService>();
        MockGraphService.Setup(x => x.UserExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        MockGraphService.Setup(x => x.CreateGuestUserAsync(It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("test-entra-id");

        MockGraphService.Setup(x => x.GetFifcAdmin(It.IsAny<CancellationToken>()))
            .ReturnsAsync(["O.SORO@AFDB.ORG", "M.DIARRASSOUBA@AFDB.ORG"]);

        services.AddSingleton(MockGraphService.Object);

        MockServiceBusService = new Mock<IServiceBusService>();
        services.AddSingleton(MockServiceBusService.Object);

        MockAuditService = new Mock<IAuditService>();
        services.AddSingleton(MockAuditService.Object);
    }
}


// --- Handler d'authentification factice ---
public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.NoResult());

        // Rôles
        var rolesHeader = Context.Request.Headers["X-Test-Roles"].FirstOrDefault()
                       ?? Context.Request.Headers["X-Test-Role"].FirstOrDefault();

        var roles = string.IsNullOrWhiteSpace(rolesHeader) ? []
            : rolesHeader.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);



        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, "test-user-id"),
            new (ClaimTypes.Name, "Test User"),
            new (ClaimTypes.Email, "admintest@afdb.com")
        };

        foreach (var r in roles)
            claims.Add(new Claim(ClaimTypes.Role, r));

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
