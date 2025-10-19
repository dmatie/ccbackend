using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class ClaimCreatedEvent : DomainEvent
{
    public Guid ClaimId { get; }
    public string ClaimTypeEn { get; }
    public string ClaimTypeFr { get; }
    public string Comment { get; }
    public string Country { get; }
    public string AuthorFirstName { get; }
    public string AuthorLastName { get; }
    public string AuthorEmail { get; }
    public string[] AssignToEmail { get; }
    public string[] AssignCcEmail { get; }

    public ClaimCreatedEvent(Guid claimId, Country country, ClaimType claimType,
        User ClaimAuthor, string[] assignToEmail, string[] assignCcEmail, string comment)
    {
        ClaimId = claimId;
        ClaimTypeEn = claimType.Name;
        ClaimTypeFr = claimType.NameFr;
        AuthorFirstName = ClaimAuthor.FirstName;
        AuthorLastName = ClaimAuthor.LastName;
        AuthorEmail = ClaimAuthor.Email;
        AssignToEmail = assignToEmail;
        AssignCcEmail = assignCcEmail;
        Comment = comment;
        Country = country.Name;
    }
}
