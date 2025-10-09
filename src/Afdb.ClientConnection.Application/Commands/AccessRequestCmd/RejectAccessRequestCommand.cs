using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed record RejectAccessRequestCommand : IRequest<RejectAccessRequestResponse>
{
    public Guid AccessRequestId { get; set; }
    public string RejectionReason { get; set; } = string.Empty;
}

public sealed record RejectAccessRequestResponse
{
    public AccessRequestDto AccessRequest { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}