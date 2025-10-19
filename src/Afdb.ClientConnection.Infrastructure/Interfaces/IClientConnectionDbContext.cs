using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Interfaces;

public interface IClientConnectionDbContext
{
    DbSet<Domain.Entities.User> Users { get; }
    public DbSet<AccessRequestEntity> AccessRequests { get; }
    DbSet<Domain.Entities.AuditLog> AuditLogs { get; }
    public DbSet<FunctionEntity> Functions { get; }
    public DbSet<CountryEntity> Countries { get; }
    public DbSet<BusinessProfileEntity> BusinessProfiles { get; }
    DbSet<FinancingTypeEntity> FinancingTypes { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}