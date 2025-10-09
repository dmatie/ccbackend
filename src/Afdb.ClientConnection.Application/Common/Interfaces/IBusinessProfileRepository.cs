using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IBusinessProfileRepository
{
    Task<BusinessProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BusinessProfile>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BusinessProfile>> GetActiveAsync(CancellationToken cancellationToken = default);
}
