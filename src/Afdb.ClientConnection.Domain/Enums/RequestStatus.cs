namespace Afdb.ClientConnection.Domain.Enums;
public enum RequestStatus
{
    Draft = 0,
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Completed = 10,
    Cancelled = 20,
    ApprovedByApp = 22
}