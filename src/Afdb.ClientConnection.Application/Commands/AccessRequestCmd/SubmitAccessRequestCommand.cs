using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed record SubmitAccessRequestCommand : IRequest<SubmitAccessRequestResponse>
{
    public Guid AccessRequestId { get; set; }
}

public sealed record SubmitAccessRequestResponse
{
    public AccessRequestDto AccessRequest { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
