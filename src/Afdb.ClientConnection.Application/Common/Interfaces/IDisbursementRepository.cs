using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IDisbursementRepository
{
    Task<Disbursement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Disbursement?> GetByRequestNumberAsync(string requestNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Disbursement>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Disbursement>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Disbursement> AddAsync(Disbursement disbursement, CancellationToken cancellationToken = default);
    Task<Disbursement> UpdateAsync(Disbursement disbursement, CancellationToken cancellationToken = default);
    Task<string> GenerateRequestNumberAsync(CancellationToken cancellationToken = default);
}
