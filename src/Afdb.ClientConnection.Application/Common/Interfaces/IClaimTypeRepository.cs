using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IClaimTypeRepository
{
    Task<bool> ExistsAsync(Guid id);
    Task<IEnumerable<ClaimType>?> GetAllAsync();
    Task<ClaimType?> GetByIdAsync(Guid id);
}
