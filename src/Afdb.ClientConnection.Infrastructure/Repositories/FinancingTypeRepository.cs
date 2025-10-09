using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

public class FinancingTypeRepository : IFinancingTypeRepository
{
    private readonly ClientConnectionDbContext _context;

    public FinancingTypeRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<FinancingType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FinancingTypes
            .Where(ft => ft.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return entity != null ? new FinancingType(entity.Id, entity.Name, entity.Code, entity.Description, entity.CreatedBy) : null;
    }

    public async Task<IEnumerable<FinancingType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.FinancingTypes
            .OrderBy(ft => ft.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(e => new FinancingType(e.Id, e.Name, e.Code, e.Description, e.CreatedBy));
    }

    public async Task<IEnumerable<FinancingType>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.FinancingTypes
            .Where(ft => ft.IsActive)
            .OrderBy(ft => ft.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(e => new FinancingType(e.Id, e.Name, e.Code, e.Description, e.CreatedBy));
    }
}
