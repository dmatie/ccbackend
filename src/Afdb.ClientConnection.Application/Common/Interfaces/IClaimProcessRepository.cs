using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IClaimProcessRepository
{
    Task<ClaimProcess> AddAsync(ClaimProcess claimProcess);
    Task<ClaimProcess?> GetByIdAsync(Guid id);
    Task<IEnumerable<ClaimProcess>?> GetByClaimIdAsync(Guid claimId);
}
