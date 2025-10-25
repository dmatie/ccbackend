using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class CurrencyRepository : ICurrencyRepository
{
    private readonly ClientConnectionDbContext _context;

    public CurrencyRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<Currency?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Currencies
            .FirstOrDefaultAsync(c => c.Id == id);

        return entity == null ? null : DomainMappings.MapCurrencyToDomain(entity);
    }

    public async Task<IEnumerable<Currency>?> GetAllAsync()
    {
        var entities = await _context.Currencies
            .OrderBy(c => c.Name)
            .ToListAsync();

        if (entities.Count == 0)
            return null;

        return entities.Select(DomainMappings.MapCurrencyToDomain);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Currencies.AnyAsync(c => c.Id == id);
    }
}