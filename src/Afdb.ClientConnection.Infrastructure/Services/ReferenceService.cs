using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Infrastructure.Services;
public class ReferenceService : IReferenceService
{
    private readonly IFunctionRepository _functionRepository;
    private readonly IBusinessProfileRepository _businessProfileRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IFinancingTypeRepository _financingTypeRepository;

    public ReferenceService(
        IFunctionRepository functionRepository,
        IBusinessProfileRepository businessProfileRepository,
        ICountryRepository countryRepository,
        IFinancingTypeRepository financingTypeRepository)
    {
        _functionRepository = functionRepository;
        _businessProfileRepository = businessProfileRepository;
        _countryRepository = countryRepository;
        _financingTypeRepository = financingTypeRepository;
    }

    public async Task<Function?> GetFunctionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _functionRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<BusinessProfile?> GetBusinessProfileByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _businessProfileRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Country?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _countryRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Function>> GetActiveFunctionsAsync(CancellationToken cancellationToken = default)
    {
        return await _functionRepository.GetActiveAsync(cancellationToken);
    }

    public async Task<IEnumerable<BusinessProfile>> GetActiveBusinessProfilesAsync(CancellationToken cancellationToken = default)
    {
        return await _businessProfileRepository.GetActiveAsync(cancellationToken);
    }

    public async Task<IEnumerable<Country>> GetActiveCountriesAsync(CancellationToken cancellationToken = default)
    {
        return await _countryRepository.GetActiveAsync(cancellationToken);
    }

    public async Task<FinancingType?> GetFinancingTypeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _financingTypeRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<FinancingType>> GetActiveFinancingTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _financingTypeRepository.GetActiveAsync(cancellationToken);
    }
}
