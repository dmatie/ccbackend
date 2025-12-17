using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    public async Task<IEnumerable<Claim>?> GetAllAsync(UserContext userContext)
    {
        var query = _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes).ThenInclude(pr => pr.User)
            .AsQueryable();

        if (userContext.RequiresCountryFilter)
        {
            query = query.Where(c => userContext.CountryIds.Contains(c.CountryId));
        }

        var entities = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        if (entities.Count == 0)
            return null;

        return [.. entities.Select(DomainMappings.MapClaimToDomain)];
    }

    public async Task<IEnumerable<Claim>?> GetAllByStatusAsync(ClaimStatus? status, UserContext userContext)
    {

        var query = _context.Claims
        .Include(c => c.ClaimType)
        .Include(c => c.Country)
        .Include(c => c.User)
        .Include(c => c.Processes).ThenInclude(pr => pr.User)
        .AsQueryable();

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        if (userContext.RequiresCountryFilter)
        {
            query = query.Where(c => userContext.CountryIds.Contains(c.CountryId));
        }

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

    public async Task<int> CountByStatusAsync(List<ClaimStatus> status, CancellationToken cancellationToken = default)
    {
        return await _context.Claims
            .Where(c => status.Contains(c.Status))
            .CountAsync(cancellationToken);
    }

    public async Task<int> CountByStatusAsync(UserContext userContext, List<ClaimStatus> status, CancellationToken cancellationToken = default)
    {
        var query = _context.Claims
            .Where(c => status.Contains(c.Status));

        if (userContext.RequiresCountryFilter)
        {
            query = query.Where(c => userContext.CountryIds.Contains(c.CountryId));
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<int> CountByUserIdAndStatusAsync(Guid userId, ClaimStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Claims
            .Where(c => c.UserId == userId && c.Status == status)
            .CountAsync(cancellationToken);
    }

    public async Task<(List<Claim> items, int totalCount)> GetWithFiltersAndPaginationAsync(
        UserContext userContext,
        ClaimStatus? status,
        Guid? claimTypeId,
        Guid? countryId,
        DateTime? createdFrom,
        DateTime? createdTo,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes).ThenInclude(pr => pr.User)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (claimTypeId.HasValue)
        {
            query = query.Where(c => c.ClaimTypeId == claimTypeId.Value);
        }

        if (countryId.HasValue)
        {
            query = query.Where(c => c.CountryId == countryId.Value);
        }

        if (createdFrom.HasValue)
        {
            query = query.Where(c => c.CreatedAt >= createdFrom.Value);
        }

        if (createdTo.HasValue)
        {
            query = query.Where(c => c.CreatedAt <= createdTo.Value);
        }

        if (userContext.RequiresCountryFilter)
        {
            query = query.Where(c => userContext.CountryIds.Contains(c.CountryId));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = entities.Select(DomainMappings.MapClaimToDomain).ToList();

        return (items, totalCount);
    }

    public async Task<(List<Claim> items, int totalCount)> GetByUserIdWithFiltersAndPaginationAsync(
       Guid userId,
       ClaimStatus? status,
       Guid? claimTypeId,
       Guid? countryId,
       DateTime? createdFrom,
       DateTime? createdTo,
       int pageNumber,
       int pageSize,
       CancellationToken cancellationToken = default)
    {
        var query = _context.Claims
            .Include(c => c.ClaimType)
            .Include(c => c.Country)
            .Include(c => c.User)
            .Include(c => c.Processes).ThenInclude(pr => pr.User)
            .Where(c => c.UserId == userId);

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (claimTypeId.HasValue)
        {
            query = query.Where(c => c.ClaimTypeId == claimTypeId.Value);
        }

        if (countryId.HasValue)
        {
            query = query.Where(c => c.CountryId == countryId.Value);
        }

        if (createdFrom.HasValue)
        {
            query = query.Where(c => c.CreatedAt >= createdFrom.Value);
        }

        if (createdTo.HasValue)
        {
            query = query.Where(c => c.CreatedAt <= createdTo.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = entities.Select(DomainMappings.MapClaimToDomain).ToList();

        return (items, totalCount);
    }
}
