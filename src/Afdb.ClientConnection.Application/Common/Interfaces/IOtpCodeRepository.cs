using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IOtpCodeRepository
{
    Task AddOtpAsync(OtpCode otp);
    Task DeleteOtpsByEmailAsync(string email);
    Task<OtpCode?> GetValidOtpByEmailAndCodeAsync(string email, string code);
    Task<OtpCode?> GetValidOtpByEmailAsync(string email);
}
