using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Afdb.ClientConnection.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

public class BusinessProfileRepository : IBusinessProfileRepository
{
    private readonly ClientConnectionDbContext _context;

    public BusinessProfileRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<BusinessProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.BusinessProfiles
            .Where(bp => bp.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return entity != null ? DomainMappings.MapBusinessProfile(entity) : null;
    }

    public async Task<IEnumerable<BusinessProfile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.BusinessProfiles
            .OrderBy(bp => bp.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapBusinessProfile);
    }

    public async Task<IEnumerable<BusinessProfile>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.BusinessProfiles
            .Where(bp => bp.IsActive)
            .OrderBy(bp => bp.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapBusinessProfile);
    }
}
