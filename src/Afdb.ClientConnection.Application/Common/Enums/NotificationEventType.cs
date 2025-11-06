namespace Afdb.ClientConnection.Application.Common.Enums;

public enum NotificationEventType
{
    ClaimCreated,
    ClaimSubmitted,
    ClaimApproved,
    ClaimRejected,
    ClaimResponseAdded,

    DisbursementCreated,
    DisbursementSubmitted,
    DisbursementApproved,
    DisbursementRejected,
    DisbursementBackedToClient,
    DisbursementReSubmitted,

    AccessRequestCreated,
    AccessRequestApproved,
    AccessRequestRejected
}
