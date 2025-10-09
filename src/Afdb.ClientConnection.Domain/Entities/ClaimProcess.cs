using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class ClaimProcess : AggregateRoot
{
    public Guid ClaimId { get; private set; }
    public Guid UserId { get; private set; }
    public ClaimStaus Status { get; set; }
    public string Comment { get; private set; }
    public User User { get; private set; }= default!;

    public ClaimProcess(Guid claimId, Guid userId, string comment, User user)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment cannot be empty");

        ClaimId = claimId;
        UserId = userId;
        Comment = comment;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = user.Email;
    }

    public ClaimProcess(ClaimProcessLoadParam loadParam)
    {
        Id = loadParam.Id;
        ClaimId = loadParam.ClaimId;
        UserId = loadParam.UserId;
        User = loadParam.User;
        Comment = loadParam.Comment;
        CreatedAt = loadParam.CreatedAt;
        UpdatedAt = loadParam.UpdatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedBy = loadParam.UpdatedBy;
    }
}