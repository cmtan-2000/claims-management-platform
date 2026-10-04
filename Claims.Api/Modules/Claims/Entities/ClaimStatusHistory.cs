namespace Claims.Api.Modules.Claims.Entities;

public class ClaimStatusHistory
{
    public Guid Id { get; set; }

    public Guid ClaimId { get; set; }

    public ClaimStatus FromStatus { get; set; }

    public ClaimStatus ToStatus { get; set; }

    public Guid ChangedByOfficerId { get; set; }

    public DateTime ChangedAt { get; set; }

    public string? Reason { get; set; }
}