using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

public sealed class DisbursementPermissionRepository : IDisbursementPermissionRepository
{
    private readonly ClientConnectionDbContext _context;

    public DisbursementPermissionRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<DisbursementPermission?> GetByBusinessProfileAndFunctionAsync(
        Guid businessProfileId,
        Guid functionId,
        CancellationToken cancellationToken)
    {
        var entity = await _context.DisbursementPermissions
            .FirstOrDefaultAsync(
                p => p.BusinessProfileId == businessProfileId && p.FunctionId == functionId,
                cancellationToken);

        return entity?.ToDomain();
    }

    public async Task<List<Guid>> GetAuthorizedBusinessProfileIdsAsync(
        Guid functionId,
        CancellationToken cancellationToken)
    {
        return await _context.DisbursementPermissions
            .Where(p => p.FunctionId == functionId && p.CanConsult)
            .Select(p => p.BusinessProfileId)
            .ToListAsync(cancellationToken);
    }
}
