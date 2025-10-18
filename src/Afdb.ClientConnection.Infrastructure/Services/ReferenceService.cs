using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Infrastructure.Services;
public class ReferenceService(
    IFunctionRepository functionRepository,
    IBusinessProfileRepository businessProfileRepository,
    ICountryRepository countryRepository,
    IClaimTypeRepository claimTypeRepository,
    IFinancingTypeRepository financingTypeRepository) : IReferenceService
{
    private readonly IFunctionRepository _functionRepository = functionRepository;
    private readonly IBusinessProfileRepository _businessProfileRepository = businessProfileRepository;
    private readonly ICountryRepository _countryRepository = countryRepository;
    private readonly IFinancingTypeRepository _financingTypeRepository = financingTypeRepository;
    private readonly IClaimTypeRepository _claimTypeRepository = claimTypeRepository;

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

    public async Task<ClaimType?> GetClaimTypeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _claimTypeRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ClaimType>?> GetActiveClaimTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _claimTypeRepository.GetAllAsync();
    }
}
