using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly ClientConnectionDbContext _context;

    public CountryRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<Country?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Countries
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return entity != null ? new Country(entity.Id, entity.Name, entity.NameFr, entity.Code, entity.CreatedBy) : null;
    }

    public async Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Countries
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(e => new Country(e.Id, e.Name, e.NameFr, e.Code, e.CreatedBy));
    }

    public async Task<IEnumerable<Country>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Countries
            .Where(c => c.IsActive && c.Code != "NOT")
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(e => new Country(e.Id, e.Name, e.NameFr, e.Code, e.CreatedBy));
    }

    public async Task<Country?> GetDefaultCountryAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _context.Countries
            .Where(c => !c.IsActive && c.Code == "NOT")
            .FirstOrDefaultAsync(cancellationToken);

        return entity != null ? new Country(entity.Id, entity.Name, entity.NameFr, entity.Code, entity.CreatedBy) : null;
    }
}
