using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class ClaimProcessRepository : IClaimProcessRepository
{
    private readonly ClientConnectionDbContext _context;

    public ClaimProcessRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimProcess> AddAsync(ClaimProcess claimProcess)
    {
        var entity = EntityMappings.MapClaimProcessToEntity(claimProcess);
        
        _context.ClaimProcesses.Add(entity);
        await _context.SaveChangesAsync();

        var entityWithRelations = await _context.ClaimProcesses
            .Include(cp => cp.User)
            .FirstOrDefaultAsync(cp => cp.Id == entity.Id);

        if (entityWithRelations != null)
        {
            return DomainMappings.MapClaimProcessToDomain(entityWithRelations)!;
        }

        return DomainMappings.MapClaimProcessToDomain(entity)!;
    }

    public async Task<ClaimProcess?> GetByIdAsync(Guid id)
    {
        var entity = await _context.ClaimProcesses
            .Include(cp => cp.User)
            .FirstOrDefaultAsync(cp => cp.Id == id);

        return entity == null ? null : DomainMappings.MapClaimProcessToDomain(entity);
    }

    public async Task<IEnumerable<ClaimProcess>?> GetByClaimIdAsync(Guid claimId)
    {
        var entities = await _context.ClaimProcesses
            .Include(cp => cp.User)
            .Where(cp => cp.ClaimId == claimId)
            .OrderBy(cp => cp.CreatedAt)
            .ToListAsync();

        if (entities.Count == 0)
            return null;

        return [.. entities.Select(DomainMappings.MapClaimProcessToDomain)!];
    }
}
