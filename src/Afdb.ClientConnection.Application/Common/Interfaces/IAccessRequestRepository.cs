using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IAccessRequestRepository
{
    Task<AccessRequest?> GetByIdAsync(Guid id);
    Task<AccessRequest?> GetByEmailAsync(string email);
    Task<AccessRequest?> GetByEmailAndStatusAsync(string email, RequestStatus status);
    Task<IEnumerable<AccessRequest>> GetAllAsync();
    Task<IEnumerable<AccessRequest>> GetByStatusAsync(RequestStatus status);
    Task<IEnumerable<AccessRequest>> GetPendingRequestsAsync();
    Task<AccessRequest> AddAsync(AccessRequest accessRequest);
    Task UpdateAsync(AccessRequest accessRequest);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailHasPendingRequestAsync(string email);
    Task<bool> EmailHasStatusRequestAsync(string email, RequestStatus status);
    Task ExecuteInTransactionAsync(Func<Task> action);
    Task<bool> ExistsEmailAsync(string email);
    Task<int> CountByStatusAsync(RequestStatus status, CancellationToken cancellationToken = default);
    Task<int> CountProjectsByUserIdAsync(string email, CancellationToken cancellationToken = default);
}