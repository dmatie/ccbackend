using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IClaimRepository
{
    Task<Claim> AddAsync(Claim claim);
    Task<bool> ExistsAsync(Guid id);
    Task<IEnumerable<Claim>?> GetAllAsync();
    Task<IEnumerable<Claim>?> GetAllByStatusAsync(ClaimStatus? status);
    Task<Claim?> GetByIdAsync(Guid id);
    Task<IEnumerable<Claim>?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Claim>?> GetByUserIdAndStatusAsync(Guid userId, ClaimStatus? status);
    Task UpdateAsync(Claim claim);
}
