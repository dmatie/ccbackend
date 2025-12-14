using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;

public sealed record GetOtherDocumentTypesQuery : IRequest<List<OtherDocumentTypeDto>>;
