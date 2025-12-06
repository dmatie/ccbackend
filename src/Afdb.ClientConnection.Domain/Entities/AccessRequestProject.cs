using Afdb.ClientConnection.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class AccessRequestProject : AggregateRoot
{
    public Guid AccessRequestId { get; set; }
    public string SapCode { get; set; } = default!;
    public string ProjectTitle { get; set; } = default!;
    public AccessRequest AccessRequest { get; set; } = default!;

    private AccessRequestProject() { } // For EF Core

    public AccessRequestProject(Guid accessRequestId, string sapCode, string title)
    {
        AccessRequestId = accessRequestId;
        SapCode = sapCode;
        ProjectTitle = title;
    }
}
