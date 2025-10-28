using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IDisbursementRepository
{
    Task<Disbursement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Disbursement?> GetByIdAsync(Guid id, DisbursementLoadParam loadParam);
    Task<Disbursement?> GetByRequestNumberAsync(string requestNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Disbursement>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Disbursement>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Disbursement> AddAsync(Disbursement disbursement, CancellationToken cancellationToken = default);
    Task<Disbursement> UpdateAsync(Disbursement disbursement, CancellationToken cancellationToken = default);
    Task<Disbursement> UpdateWithRelatedEntitiesAsync(
        Disbursement disbursement,
        DisbursementA1? newA1,
        DisbursementA2? newA2,
        DisbursementA3? newA3,
        DisbursementB1? newB1,
        CancellationToken cancellationToken = default);
    Task<string> GenerateRequestNumberAsync(CancellationToken cancellationToken = default);
    Task<Disbursement> UpdateProcessAsync(Disbursement disbursement, CancellationToken cancellationToken = default);
    Task<int> CountByStatusAsync(DisbursementStatus status, CancellationToken cancellationToken = default);
    Task<int> CountByUserIdAndStatusAsync(Guid userId, DisbursementStatus status, CancellationToken cancellationToken = default);
}
