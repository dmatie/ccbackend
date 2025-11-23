using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed record ApproveAccessRequestByAppCommand : IRequest<ApproveAccessRequestByAppResponse>
{
    public Guid AccessRequestId { get; set; }
    public string? Comments { get; set; }
    public bool IsFromApplication { get; set; }
    public string ApproverEmail { get; set; } = string.Empty;
}

public sealed record ApproveAccessRequestByAppResponse
{
    public AccessRequestDto AccessRequest { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
