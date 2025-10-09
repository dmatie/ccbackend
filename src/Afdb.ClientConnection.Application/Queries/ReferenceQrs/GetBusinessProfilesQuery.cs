using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed record GetBusinessProfilesQuery : IRequest<GetBusinessProfilesResponse>
{

}

public sealed record GetBusinessProfilesResponse
{
    public List<BusinessProfileDto> BusinessProfiles { get; init; } = new();
    public int TotalCount { get; init; }
}
