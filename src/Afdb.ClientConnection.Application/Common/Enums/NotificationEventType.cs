namespace Afdb.ClientConnection.Application.Common.Enums;

public enum NotificationEventType
{
    ClaimCreated,
    ClaimCreatedAuthor,
    ClaimSubmitted,
    ClaimApproved,
    ClaimRejected,
    ClaimResponseAdded,

    DisbursementCreated,
    DisbursementSubmitted,
    DisbursementSubmittedAuthor,
    DisbursementApproved,
    DisbursementRejected,
    DisbursementBackedToClient,
    DisbursementReSubmitted,

    AccessRequestCreated,
    AccessRequestApproved,
    AccessRequestRejected,

    OtpCreated
}
