using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface ICountryRepository
{
    Task<Country?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Country>> GetActiveAsync(CancellationToken cancellationToken = default);
}
