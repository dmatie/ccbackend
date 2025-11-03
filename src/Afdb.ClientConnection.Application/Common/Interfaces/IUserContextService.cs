using Afdb.ClientConnection.Application.Common.Models;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IUserContextService
{
    UserContext GetUserContext();
    bool HasUserContext();
}
