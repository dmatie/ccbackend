using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Afdb.ClientConnection.Infrastructure.Repositories;
using System.Security.Cryptography;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class OtpService : IOtpService
{
    private readonly IOtpCodeRepository _repository;

    public OtpService(IOtpCodeRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> GenerateOtpForEmailAsync(string email)
    {
        // Supprimer les anciens OTP pour cet email
        await _repository.DeleteOtpsByEmailAsync(email);

        var code = GenerateRandomCode();
        var now = DateTime.UtcNow;

        var otp = new OtpCode(email, code, now.AddMinutes(10));

        await _repository.AddOtpAsync(otp);
        return code;
    }
    public async Task<bool> IsOtpValidAsync(string email, string code)
    {
        var otp = await _repository.GetValidOtpByEmailAndCodeAsync(email, code);
        return otp != null;
    }

    private static string GenerateRandomCode()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var value = BitConverter.ToUInt32(bytes, 0) % 1000000;
        return value.ToString("D6");
    }
}
