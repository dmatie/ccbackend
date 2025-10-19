using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed record CreateAccessRequestCommand : IRequest<CreateAccessRequestResponse>
{
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public Guid? FunctionId { get; init; }
    public Guid? CountryId { get; init; }
    public Guid? BusinessProfileId { get; init; }
    public Guid? FinancingTypeId { get; init; }
    public List<RequestProject> Projects { get; init; } = [];
}

public sealed record RequestProject
{
    public string SapCode { get; init; } = default!;
}

public sealed record CreateAccessRequestResponse
{
    public AccessRequestDto AccessRequest { get; init; } = new();
    public string Message { get; init; } = string.Empty;
}