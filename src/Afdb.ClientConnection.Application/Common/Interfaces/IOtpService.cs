
namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IOtpService
{
    Task<string> GenerateOtpForEmailAsync(string email);
    Task<bool> IsOtpValidAsync(string email, string code);
}
