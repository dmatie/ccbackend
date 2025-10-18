using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;

namespace Afdb.ClientConnection.Application.Commands.ClaimCmd;

public sealed record CreateClaimCommand : IRequest<CreateClaimResponse>
{
    public Guid ClaimTypeId { get; init; }
    public string Comment { get; init; } = string.Empty;
}

public sealed record CreateClaimResponse
{
    public ClaimDto Claim { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
