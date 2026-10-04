namespace Claims.Api.Modules.Claims.Entities;

public enum ClaimStatus
{
    Submitted,
    UnderReview,
    AwaitingInformation,
    PendingAssessment,
    PendingSettlement,
    Approved,
    Rejected,
    Settled,
    UnderAssessment
}