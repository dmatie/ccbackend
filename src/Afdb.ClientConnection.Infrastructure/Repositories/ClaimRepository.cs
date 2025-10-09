using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using AutoMapper;
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
            .Include(c => c.Processes)
            .FirstOrDefaultAsync(c => c.Id == id);

        return entity == null ? null : DomainMappings.MapClaimToDomain(entity);
    }

    public async Task<IEnumerable<Claim>?> GetAllAsync()
    {
        var entities = await _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        if (entities.Count == 0)
            return null;

        return [.. entities.Select(DomainMappings.MapClaimToDomain)];
    }


    public async Task<IEnumerable<Claim>?> GetAllByStatusAsync(ClaimStaus status)
    {
        var entities = await _context.Claims
            .Where(c => c.Status == status)
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        if(entities.Count == 0)
            return null;

        return [.. entities.Select(DomainMappings.MapClaimToDomain)];
    }

    public async Task<IEnumerable<Claim>?> GetByUserIdAsync(Guid userId)
    {
        var entities = await _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes)
            .Where(c => c.UserId == userId)
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
            .FirstOrDefaultAsync(c => c.Id == claim.Id);

        if (entity != null)
        {
            entity.Processes.Clear();
            EntityMappings.UpdateClaimEntityFromDomain(entity, claim);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Claims.AnyAsync(c => c.Id == id);
    }
}
