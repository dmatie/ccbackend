using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
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

        return entity != null ? DomainMappings.MapFunction(entity) : null;
    }

    public async Task<IEnumerable<Function>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Functions
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapFunction);
    }

    public async Task<IEnumerable<Function>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Functions
            .Where(f => f.IsActive)
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapFunction);
    }
}
