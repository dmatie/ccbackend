using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.ClaimCmd;

public sealed record AddClaimResponseCommand : IRequest<ClaimDto>
{
    public Guid ClaimId { get; init; }
    public Guid UserId { get; init; }
    public ClaimStaus Status { get; init; }
    public string Comment { get; init; } = string.Empty;
}
