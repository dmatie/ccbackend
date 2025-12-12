using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IClaimRepository
{
    Task<Claim> AddAsync(Claim claim);
    Task<bool> ExistsAsync(Guid id);
    Task<IEnumerable<Claim>?> GetAllAsync(UserContext userContext);
    Task<IEnumerable<Claim>?> GetAllByStatusAsync(ClaimStatus? status, UserContext userContext);
    Task<Claim?> GetByIdAsync(Guid id);
    Task<IEnumerable<Claim>?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Claim>?> GetByUserIdAndStatusAsync(Guid userId, ClaimStatus? status);
    Task UpdateAsync(Claim claim);
    Task<int> CountByStatusAsync(List<ClaimStatus> status, CancellationToken cancellationToken = default);
    Task<int> CountByStatusAsync(UserContext userContext, List<ClaimStatus> status, CancellationToken cancellationToken = default);
    Task<int> CountByUserIdAndStatusAsync(Guid userId, ClaimStatus status, CancellationToken cancellationToken = default);

    Task<(List<Claim> items, int totalCount)> GetWithFiltersAndPaginationAsync(
        UserContext userContext,
        ClaimStatus? status,
        Guid? claimTypeId,
        Guid? countryId,
        DateTime? createdFrom,
        DateTime? createdTo,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
