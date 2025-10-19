using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Afdb.ClientConnection.Infrastructure.Data;

public class ClientConnectionDbContextFactory : IDesignTimeDbContextFactory<ClientConnectionDbContext>
{
    public ClientConnectionDbContext CreateDbContext(string[] args)
    {
        // Charger la configuration depuis le dossier racine
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(GetProjectPath()) // ou ../Afdb.ClientConnection.Api si nécessaire
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DbConnectionString");

        var optionsBuilder = new DbContextOptionsBuilder<ClientConnectionDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ClientConnectionDbContext(optionsBuilder.Options, new NoOpMediator());
    }

    private static string GetProjectPath()
    {
        // On remonte vers le projet API pour trouver le appsettings.json
        var currentDir = Directory.GetCurrentDirectory();
        var projectPath = Path.Combine(currentDir, "../../src/Afdb.ClientConnection.Api");
        return projectPath;
    }

    // IMediator vide, juste pour design-time
    private class NoOpMediator : IMediator
    {
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            throw new NotImplementedException();
        }

        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
