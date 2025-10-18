using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.ClaimCmd;

public sealed record AddClaimResponseCommand : IRequest<AddClaimResponseResponse>
{
    public Guid ClaimId { get; init; }
    public int Status { get; init; }
    public string Comment { get; init; } = string.Empty;
}

public sealed record AddClaimResponseResponse 
{
    public ClaimDto Claim { get; init; } = default!;
    public string Message { get; init; } = default!;
}
