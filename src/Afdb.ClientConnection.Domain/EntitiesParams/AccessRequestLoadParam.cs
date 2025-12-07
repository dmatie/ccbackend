using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record AccessRequestLoadParam : CommonLoadParam
{
    public RequestStatus Status { get; init; }
    public string Email { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Code { get; init; } = default!;
    public string[] ApproversEmail { get; init; } = [];
    public Guid? FunctionId { get; init; }
    public Guid? CountryId { get; init; }
    public Guid? BusinessProfileId { get; init; }
    public Guid? FinancingTypeId { get; init; }
    public DateTime RequestedDate { get; init; }
    public DateTime? ProcessedDate { get;init; }
    public Guid? ProcessedById { get; init; }
    public string? ProcessingComments { get; init; }
    public User? ProcessedBy { get;  init; }
    public Function? Function { get; init; }
    public Country? Country { get; init; }
    public BusinessProfile? BusinessProfile { get; init; }
    public FinancingType? FinancingType { get; init; }
    public List<AccessRequestProject> Projects { get; init; } = [];
    public List<AccessRequestDocument> Documents { get; init; } = [];
}
