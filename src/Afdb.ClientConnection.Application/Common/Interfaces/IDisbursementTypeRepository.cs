using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IDisbursementTypeRepository
{
    Task<DisbursementType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DisbursementType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<DisbursementType>> GetAllAsync(CancellationToken cancellationToken = default);
}
