using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class OtpCodeRepository(ClientConnectionDbContext context) : IOtpCodeRepository
{
    private readonly ClientConnectionDbContext _context = context;

    public async Task<OtpCode?> GetValidOtpByEmailAsync(string email)
    {
        var otpCode= await _context.OtpCodes
            .FirstOrDefaultAsync(o => o.Email == email && o.ExpiresAt > DateTime.UtcNow)?? null;

        return otpCode == null ? null : MapToDomain(otpCode);
    }

    public async Task AddOtpAsync(OtpCode otp)
    {
        var otpEntity = MapToEntity(otp);
        _context.OtpCodes.Add(otpEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<OtpCode?> GetValidOtpByEmailAndCodeAsync(string email, string code)
    {
        var entity = await _context.OtpCodes
            .FirstOrDefaultAsync(o => o.Email == email && o.Code == code && o.ExpiresAt > DateTime.UtcNow);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task DeleteOtpsByEmailAsync(string email)
    {
        var otps = _context.OtpCodes.Where(o => o.Email == email);
        _context.OtpCodes.RemoveRange(otps);
        await _context.SaveChangesAsync();
    }

    private static OtpCodeEntity MapToEntity(OtpCode otpCode)
    {
        return new OtpCodeEntity
        {
            Id = otpCode.Id,
            Email = otpCode.Email,
            Code = otpCode.Code,
            ExpiresAt = otpCode.ExpiresAt,
            CreatedAt = otpCode.CreatedAt,
            CreatedBy = otpCode.CreatedBy,
            UpdatedAt = otpCode.UpdatedAt,
            UpdatedBy = otpCode.UpdatedBy
        };
    }

    private static OtpCode MapToDomain(OtpCodeEntity entity)
    {
        // Use reflection to create domain entity with private constructor
        var otpCode = (OtpCode)Activator.CreateInstance(typeof(OtpCode), true)!;

        // Set properties using reflection
        typeof(OtpCode).GetProperty("Id")!.SetValue(otpCode, entity.Id);
        typeof(OtpCode).GetProperty("Email")!.SetValue(otpCode, entity.Email);
        typeof(OtpCode).GetProperty("Code")!.SetValue(otpCode, entity.Code);
        typeof(OtpCode).GetProperty("ExpiresAt")!.SetValue(otpCode, entity.ExpiresAt);

        return otpCode;
    }

}
