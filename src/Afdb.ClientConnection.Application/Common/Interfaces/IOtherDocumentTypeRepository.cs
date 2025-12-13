using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IOtherDocumentTypeRepository
{
    Task<OtherDocumentType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OtherDocumentType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(OtherDocumentType otherDocumentType, CancellationToken cancellationToken = default);
    Task UpdateAsync(OtherDocumentType otherDocumentType, CancellationToken cancellationToken = default);
}
