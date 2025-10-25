using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface ICurrencyRepository
{
    Task<Currency?> GetByIdAsync(Guid id);
    Task<IEnumerable<Currency>?> GetAllAsync();
    Task<bool> ExistsAsync(Guid id);
}