using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed record GetDisbursementByIdQuery(Guid DisbursementId) : IRequest<DisbursementDto>;
