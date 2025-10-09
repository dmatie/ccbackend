using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

public class FunctionRepository : IFunctionRepository
{
    private readonly ClientConnectionDbContext _context;

    public FunctionRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<Function?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Functions
            .Where(f => f.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return entity != null ? new Function(entity.Id, entity.Name, entity.Code , entity.Description, entity.CreatedBy) : null;
    }

    public async Task<IEnumerable<Function>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Functions
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(e => new Function(e.Id, e.Name, e.Code, e.Description, e.CreatedBy));
    }

    public async Task<IEnumerable<Function>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Functions
            .Where(f => f.IsActive)
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(e => new Function(e.Id, e.Name, e.Code, e.Description, e.CreatedBy));
    }
}
