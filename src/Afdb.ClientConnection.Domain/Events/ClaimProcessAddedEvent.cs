using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class ClaimProcessAddedEvent : DomainEvent
{
    public Guid ClaimId { get; }
    public string ClaimTypeEn { get; }
    public string ClaimTypeFr { get; }
    public string Comment { get; }
    public string Country { get; }
    public string AuthorFirstName { get; }
    public string AuthorLastName { get; }
    public string AuthorEmail { get; }
    public string ProcessComment { get; }
    public string ProcessAuthorFirstName { get; }
    public string ProcessAuthorLastName { get; }
    public string ProcessStatus { get; }

    public ClaimProcessAddedEvent(Guid claimId, Country country,ClaimType claimType, User ClaimAuthor, string claimComment,
        User processAuthor , string processComment, string processStatus)
    {
        ClaimId = claimId;
        ClaimTypeEn = claimType.Name;
        ClaimTypeFr = claimType.NameFr;
        AuthorFirstName = ClaimAuthor.FirstName;
        AuthorLastName = ClaimAuthor.LastName;
        AuthorEmail = ClaimAuthor.Email;
        Comment = claimComment;
        Country = country.Name;
        ProcessAuthorFirstName = processAuthor.FirstName;
        ProcessAuthorLastName = processAuthor.LastName;
        ProcessComment = processComment;
        ProcessStatus = processStatus;
    }
}
