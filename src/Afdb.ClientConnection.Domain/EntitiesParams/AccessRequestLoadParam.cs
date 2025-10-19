using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record AccessRequestLoadParam
{
    public Guid Id { get; init; }
    public string Email { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string CreatedBy { get; init; } = default!;
    public string[] ApproversEmail { get; init; } = [];
    public Guid? FunctionId { get; init; }
    public Guid? CountryId { get; init; }
    public Guid? BusinessProfileId { get; init; }
    public Guid? FinancingTypeId { get; init; }
    public Function? Function { get; init; }
    public Country? Country { get; init; }
    public BusinessProfile? BusinessProfile { get; init; }
    public FinancingType? FinancingType { get; init; }
    public List<AccessRequestProject> Projects { get; init; } = [];
}
