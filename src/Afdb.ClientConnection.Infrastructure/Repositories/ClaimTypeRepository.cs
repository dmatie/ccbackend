using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class ClaimTypeRepository : IClaimTypeRepository
{
    private readonly ClientConnectionDbContext _context;

    public ClaimTypeRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimType?> GetByIdAsync(Guid id)
    {
        var entity = await _context.ClaimTypes
            .FirstOrDefaultAsync(c => c.Id == id);

        return entity == null ? null : DomainMappings.MapClaimTypeToDomain(entity);
    }

    public async Task<IEnumerable<ClaimType>?> GetAllAsync()
    {
        var entities = await _context.ClaimTypes
            .OrderBy(c => c.Name)
            .ToListAsync();

        if (entities.Count == 0)
            return null;

        return [.. entities.Select(DomainMappings.MapClaimTypeToDomain)];
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.ClaimTypes.AnyAsync(c => c.Id == id);
    }
}
