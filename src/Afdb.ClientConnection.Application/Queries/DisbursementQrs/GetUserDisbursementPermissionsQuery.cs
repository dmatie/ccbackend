using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed class GetUserDisbursementPermissionsQuery : IRequest<DisbursementPermissionsDto>
{
}
