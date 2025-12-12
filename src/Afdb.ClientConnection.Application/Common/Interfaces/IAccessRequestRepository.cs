using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IAccessRequestRepository
{
    Task<AccessRequest?> GetByIdAsync(Guid id);
    Task<AccessRequest?> GetByEmailAsync(string email);
    Task<AccessRequest?> GetByEmailAndStatusAsync(string email, RequestStatus status);
    Task<IEnumerable<AccessRequest>> GetAllAsync(UserContext userContext);
    Task<IEnumerable<AccessRequest>> GetByStatusAsync(UserContext userContext, RequestStatus status);
    Task<IEnumerable<AccessRequest>> GetPendingRequestsAsync(UserContext userContext);
    Task<AccessRequest> AddAsync(AccessRequest accessRequest);
    Task UpdateAsync(AccessRequest accessRequest);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailHasPendingRequestAsync(string email);
    Task<bool> EmailHasStatusRequestAsync(string email, RequestStatus status);
    Task ExecuteInTransactionAsync(Func<Task> action);
    Task<bool> ExistsEmailAsync(string email);
    Task<int> CountByStatusAsync(RequestStatus status, CancellationToken cancellationToken = default);
    Task<int> CountByStatusAsync(UserContext userContext, RequestStatus status, CancellationToken cancellationToken = default);
    Task<int> CountProjectsByUserIdAsync(string email, CancellationToken cancellationToken = default);

    Task<(List<AccessRequest> items, int totalCount)> GetApprovedWithPaginationAsync(
        UserContext userContext,
        Guid? countryId,
        string? projectCode,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<string> GenerateUniqueCodeAsync(CancellationToken cancellationToken = default);
    Task<AccessRequest?> GetByIdAndRegistrationCodelAsync(Guid id, string code);
}