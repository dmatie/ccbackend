using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed record SubmitAccessRequestCommand : IRequest<SubmitAccessRequestResponse>
{
    public Guid AccessRequestId { get; set; }
    public IFormFile Document { get; set; } = null!;
}

public sealed record SubmitAccessRequestResponse
{
    public AccessRequestDto AccessRequest { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
