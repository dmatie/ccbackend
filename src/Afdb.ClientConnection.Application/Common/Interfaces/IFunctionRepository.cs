using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IFunctionRepository
{
    Task<Function?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Function>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Function>> GetActiveAsync(CancellationToken cancellationToken = default);
}