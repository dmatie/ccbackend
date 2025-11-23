using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed record GetFileUploadedQuery : IRequest<FileUploadedDto>
{
    public string ReferenceNumber { get; init; } = default!;
    public string FileName { get; init; } = default!;
} 
