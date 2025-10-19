using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed record GetAllDisbursementsQuery : IRequest<IEnumerable<DisbursementDto>>;
