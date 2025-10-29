using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface ICountryAdminRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid countryId);
    Task<IEnumerable<CountryAdmin>?> GetByCountryIdAsync(Guid countryId, CancellationToken token);
    Task<IEnumerable<CountryAdmin>?> GetByUserIdAsync(Guid userId, CancellationToken token);
    Task<CountryAdmin> AddAsync(CountryAdmin countryAdmin, CancellationToken cancellationToken = default);
    Task AddRangeAsync(List<CountryAdmin> countryAdmins, CancellationToken cancellationToken = default);
}
