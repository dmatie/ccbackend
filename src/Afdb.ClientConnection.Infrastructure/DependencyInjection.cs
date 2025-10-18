using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Repositories;
using Afdb.ClientConnection.Infrastructure.Services;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using System.Reflection;

namespace Afdb.ClientConnection.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ClientConnectionDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DbConnectionString");
            if (!string.IsNullOrEmpty(connectionString))
            {
                options.UseSqlServer(connectionString);
            }
        });

        // Repositories
        services.AddScoped<IAccessRequestRepository, AccessRequestRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFunctionRepository, FunctionRepository>();
        services.AddScoped<IBusinessProfileRepository, BusinessProfileRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IFinancingTypeRepository, FinancingTypeRepository>();
        services.AddScoped<IClaimRepository, ClaimRepository>();
        services.AddScoped<IClaimTypeRepository, ClaimTypeRepository>();
        services.AddScoped<IClaimProcessRepository, ClaimProcessRepository>();
        services.AddScoped<ICountryAdminRepository, CountryAdminRepository>();

        // Services
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IServiceBusService, ServiceBusService>();
        services.AddScoped<IReferenceService, ReferenceService>();
        services.AddScoped<IOtpCodeRepository, OtpCodeRepository>();
        services.AddScoped<IOtpService, OtpService>();

        // HttpClient for PowerAutomateService
        services.AddHttpClient<IPowerAutomateService, PowerAutomateService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });


        // Service Bus
        services.AddScoped<ServiceBusClient>(provider =>
        {
            var connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
            return new ServiceBusClient(connectionString);
        });

        services.AddSingleton<GraphServiceClient>(sp =>
        {
            string appRegSecretKey= configuration["KeyVault:BackendSecretName"]!;
            string clientSecret = configuration[appRegSecretKey]!;

            //Créer les credentials app-only
            var tenantId = configuration["AzureAd:TenantId"];
            var clientId = configuration["AzureAd:ClientId"]; // l’AppId backend

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            //Créer et retourner le GraphServiceClient
            return new GraphServiceClient(credential);
        });

        services.AddScoped<IGraphService, GraphService>();

        var useMock = configuration.GetSection("Sap").GetValue<bool>("UseMock");
        if (useMock)
            services.AddScoped<ISapService, SapServiceMock>();
        else
            services.AddScoped<ISapService, SapService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}