using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IDisbursementRepository
{
    Task<Disbursement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Disbursement?> GetByRequestNumberAsync(string requestNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Disbursement>> GetAllAsync(UserContext userContext, CancellationToken cancellationToken = default);
    Task<IEnumerable<Disbursement>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Disbursement>> GetByUserIdWithPermissionsAsync(Guid userId, List<Guid> authorizedBusinessProfileIds, CancellationToken cancellationToken = default);
    Task<Disbursement> AddAsync(Disbursement disbursement, CancellationToken cancellationToken = default);
    Task<Disbursement> UpdateAsync(Disbursement disbursement, CancellationToken cancellationToken = default);
    Task<string> GenerateRequestNumberAsync(CancellationToken cancellationToken = default);
    Task<Disbursement> UpdateProcessAsync(Disbursement disbursement, CancellationToken cancellationToken = default);
    Task<int> CountByStatusAsync(DisbursementStatus status, CancellationToken cancellationToken = default);
    Task<int> CountByStatusAsync(UserContext userContext, DisbursementStatus status, CancellationToken cancellationToken = default);
    Task<int> CountByUserIdAndStatusAsync(Guid userId, DisbursementStatus status, CancellationToken cancellationToken = default);
    Task<bool> IdExist(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ReferenceExist(string reference, CancellationToken cancellationToken = default);
    Task<bool> FileNameExist(string reference, string fileName, CancellationToken cancellationToken = default);
}
