namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string UserId { get; }
    string UserName { get; }
    string Email { get; }
    bool IsAuthenticated { get; }
    bool IsAppAuthentification { get; }
    IEnumerable<string> Roles { get; }
    bool IsInRole(string role);
}
