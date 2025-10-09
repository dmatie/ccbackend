using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed record ApproveAccessRequestCommand : IRequest<ApproveAccessRequestResponse>
{
    public Guid AccessRequestId { get; set; }
    public string? Comments { get; set; }
}

public sealed record ApproveAccessRequestResponse
{
    public AccessRequestDto AccessRequest { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
