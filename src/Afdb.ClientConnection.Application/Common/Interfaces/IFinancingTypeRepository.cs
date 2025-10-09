using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IFinancingTypeRepository
{
    Task<FinancingType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancingType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancingType>> GetActiveAsync(CancellationToken cancellationToken = default);
}
