using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class DisbursementTypeRepository : IDisbursementTypeRepository
{
    private readonly ClientConnectionDbContext _context;

    public DisbursementTypeRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<DisbursementType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.DisbursementTypes
            .FirstOrDefaultAsync(dt => dt.Id == id, cancellationToken);

        if (entity == null)
            return null;

        return new DisbursementType(entity.Id, entity.Code, entity.Name, entity.NameFr, entity.Description, entity.CreatedAt, entity.CreatedBy, entity.UpdatedAt, entity.UpdatedBy);
    }

    public async Task<DisbursementType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var entity = await _context.DisbursementTypes
            .FirstOrDefaultAsync(dt => dt.Code == code.ToUpper(), cancellationToken);

        if (entity == null)
            return null;

        return new DisbursementType(entity.Id, entity.Code, entity.Name, entity.NameFr, entity.Description, entity.CreatedAt, entity.CreatedBy, entity.UpdatedAt, entity.UpdatedBy);
    }

    public async Task<IEnumerable<DisbursementType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.DisbursementTypes
            .OrderBy(dt => dt.Code)
            .ToListAsync(cancellationToken);

        return entities.Select(entity => new DisbursementType(entity.Id, entity.Code, entity.Name, entity.NameFr, entity.Description, entity.CreatedAt, entity.CreatedBy, entity.UpdatedAt, entity.UpdatedBy)).ToList();
    }
}
