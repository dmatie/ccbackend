using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IDisbursementPermissionRepository
{
    Task<DisbursementPermission?> GetByBusinessProfileAndFunctionAsync(
        Guid businessProfileId,
        Guid functionId,
        CancellationToken cancellationToken);

    Task<List<Guid>> GetAuthorizedBusinessProfileIdsAsync(
        Guid functionId,
        CancellationToken cancellationToken);
}
