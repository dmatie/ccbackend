using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class CountryAdminRepository : ICountryAdminRepository
{
    private readonly ClientConnectionDbContext _context;

    public CountryAdminRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CountryAdmin>?> GetByCountryIdAsync(Guid countryId, CancellationToken token)
    {
        var query = _context.CountryAdmins
            .Include(c => c.Country)
            .Include(c => c.User)
            .Where(c => c.CountryId == countryId && c.IsActive);

        var entities = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        if (entities is null)
            return null;

        return [.. entities.Select(DomainMappings.MapCountryAdminToDomain)];
    }

    public async Task<IEnumerable<CountryAdmin>?> GetByUserIdAsync(Guid userId, CancellationToken token)
    {
        var query = _context.CountryAdmins
            .Include(c => c.Country)
            .Include(c => c.User)
            .Where(c => c.UserId == userId && c.IsActive);

        var entities = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        if (entities is null)
            return null;

        return [.. entities.Select(DomainMappings.MapCountryAdminToDomain)];
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid countryId) =>
        await _context.CountryAdmins.AnyAsync(c => c.UserId == userId && c.CountryId == countryId);

    public async Task<bool> ExistsAsync(Guid userId, List<Guid> countryIds) =>
        await _context.CountryAdmins.AnyAsync(c => c.UserId == userId && countryIds.Contains(c.CountryId));


    public async Task<CountryAdmin> AddAsync(CountryAdmin countryAdmin, CancellationToken cancellationToken = default)
    {
        var entity = EntityMappings.MapCountryAdminToEntity(countryAdmin);
        await _context.CountryAdmins.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var savedEntity = await _context.CountryAdmins
            .Include(c => c.Country)
            .Include(c => c.User)
            .FirstAsync(c => c.Id == entity.Id, cancellationToken);

        return DomainMappings.MapCountryAdminToDomain(savedEntity);
    }

    public async Task AddRangeAsync(List<CountryAdmin> countryAdmins, CancellationToken cancellationToken = default)
    {
        var entities = countryAdmins.Select(EntityMappings.MapCountryAdminToEntity).ToList();
        await _context.CountryAdmins.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
