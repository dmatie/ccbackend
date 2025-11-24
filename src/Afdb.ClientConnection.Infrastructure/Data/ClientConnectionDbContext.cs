using Afdb.ClientConnection.Infrastructure.Data.Configurations;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Afdb.ClientConnection.Infrastructure.Data;

public class ClientConnectionDbContext(DbContextOptions<ClientConnectionDbContext> options, IMediator mediator) : DbContext(options)
{
    private readonly IMediator _mediator = mediator;

    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<AccessRequestEntity> AccessRequests { get; set; } = null!;
    public DbSet<AccessRequestProjectEntity> AccessRequestProject { get; set; } = null!;
    public DbSet<AuditLogEntity> AuditLogs { get; set; } = null!;
    public DbSet<FunctionEntity> Functions { get; set; } = null!;
    public DbSet<CountryAdminEntity> CountryAdmins { get; set; } = null!;
    public DbSet<CountryEntity> Countries { get; set; } = null!;
    public DbSet<BusinessProfileEntity> BusinessProfiles { get; set; } = null!;
    public DbSet<FinancingTypeEntity> FinancingTypes { get; set; } = null!;
    public DbSet<OtpCodeEntity> OtpCodes { get; set; } = null!;
    public DbSet<ClaimEntity> Claims { get; set; } = null!;
    public DbSet<ClaimTypeEntity> ClaimTypes { get; set; } = null!;
    public DbSet<ClaimProcessEntity> ClaimProcesses { get; set; } = null!;
    public DbSet<CurrencyEntity> Currencies { get; set; } = null!;
    public DbSet<DisbursementTypeEntity> DisbursementTypes { get; set; } = null!;
    public DbSet<DisbursementEntity> Disbursements { get; set; } = null!;
    public DbSet<DisbursementProcessEntity> DisbursementProcesses { get; set; } = null!;
    public DbSet<DisbursementDocumentEntity> DisbursementDocuments { get; set; } = null!;
    public DbSet<DisbursementA1Entity> DisbursementA1 { get; set; } = null!;
    public DbSet<DisbursementA2Entity> DisbursementA2 { get; set; } = null!;
    public DbSet<DisbursementA3Entity> DisbursementA3 { get; set; } = null!;
    public DbSet<DisbursementB1Entity> DisbursementB1 { get; set; } = null!;
    public DbSet<DisbursementPermissionEntity> DisbursementPermissions { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging().EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntityConfiguration>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        // Collecter les Domain Events avant de sauvegarder
        var domainEvents = ChangeTracker.Entries<BaseEntityConfiguration>()
            .SelectMany(entry => entry.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Publier les Domain Events après la sauvegarde réussie
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        // Nettoyer les événements après publication
        foreach (var entry in ChangeTracker.Entries<BaseEntityConfiguration>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}