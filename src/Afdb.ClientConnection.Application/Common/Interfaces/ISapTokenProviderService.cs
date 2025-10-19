namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface ISapTokenProviderService
{
    Task<string> GetAccessTokenAsync();
}
