using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class ClaimRepository : IClaimRepository
{
    private readonly ClientConnectionDbContext _context;

    public ClaimRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<Claim?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes).ThenInclude(pr => pr.User)
            .FirstOrDefaultAsync(c => c.Id == id);

        return entity == null ? null : DomainMappings.MapClaimToDomain(entity);
    }

    public async Task<IEnumerable<Claim>?> GetAllAsync()
    {
        var entities = await _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes).ThenInclude(pr => pr.User)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        if (entities.Count == 0)
            return null;

        return [.. entities.Select(DomainMappings.MapClaimToDomain)];
    }


    public async Task<IEnumerable<Claim>?> GetAllByStatusAsync(ClaimStatus? status)
    {

        var query = _context.Claims
        .Include(c => c.ClaimType)
        .Include(c => c.Country)
        .Include(c => c.User)
        .Include(c => c.Processes).ThenInclude(pr => pr.User)
        .AsQueryable();

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        var entities = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        if (entities.Count == 0)
            return null;

        return [.. entities.Select(DomainMappings.MapClaimToDomain)];
    }

    public async Task<IEnumerable<Claim>?> GetByUserIdAsync(Guid userId)
    {
        var entities = await _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes).ThenInclude(pr => pr.User)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        if (entities.Count == 0)
            return null;

        return [.. entities.Select(DomainMappings.MapClaimToDomain)];
    }

    public async Task<IEnumerable<Claim>?> GetByUserIdAndStatusAsync(Guid userId, ClaimStatus? status)
    {
        var query = _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes)
                .ThenInclude(p => p.User)
            .Where(c => c.UserId == userId);

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        var entities = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        if (entities.Count == 0)
            return null;

        return [.. entities.Select(DomainMappings.MapClaimToDomain)];
    }

    public async Task<Claim> AddAsync(Claim claim)
    {
        var entity = EntityMappings.MapClaimToEntity(claim);

        entity.DomainEvents = claim.DomainEvents.ToList();

        _context.Claims.Add(entity);
        await _context.SaveChangesAsync();

        var entityWithRelations = await _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes)
            .FirstOrDefaultAsync(c => c.Id == entity.Id);

        if (entityWithRelations != null)
        {
            var domainEntity = DomainMappings.MapClaimToDomain(entityWithRelations);
            return domainEntity;
        }

        return DomainMappings.MapClaimToDomain(entity);
    }

    public async Task UpdateAsync(Claim claim)
    {
        var entity = await _context.Claims
            .Include(c => c.Processes)
            .AsTracking()
            .FirstOrDefaultAsync(c => c.Id == claim.Id);

        if (entity != null)
        {
            EntityMappings.UpdateClaimEntityFromDomain(entity, claim);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id) => await _context.Claims.AnyAsync(c => c.Id == id);

    public async Task<int> CountByStatusAsync(ClaimStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Claims
            .Where(c => c.Status == status)
            .CountAsync(cancellationToken);
    }

    public async Task<int> CountByUserIdAndStatusAsync(Guid userId, ClaimStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Claims
            .Where(c => c.UserId == userId && c.Status == status)
            .CountAsync(cancellationToken);
    }
}
