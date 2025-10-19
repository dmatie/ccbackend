using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public sealed record GetUsersQuery : IRequest<GetUsersResponse>
{
    public UserRole? Role { get; init; }
    public bool? IsActive { get; init; }
    public string? OrganizationName { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public sealed record GetUsersResponse
{
    public List<UserDto> Users { get; init; } = new();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}