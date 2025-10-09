using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IReferenceService
{
    Task<Function?> GetFunctionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BusinessProfile?> GetBusinessProfileByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Country?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FinancingType?> GetFinancingTypeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Function>> GetActiveFunctionsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BusinessProfile>> GetActiveBusinessProfilesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Country>> GetActiveCountriesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancingType>> GetActiveFinancingTypesAsync(CancellationToken cancellationToken = default);
}
