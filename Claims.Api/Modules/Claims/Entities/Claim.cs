namespace Claims.Api.Modules.Claims.Entities;

public class Claim
{
    public Guid Id { get; set; }

    public string ClaimNumber { get; set; } = null!;

    public Guid ClaimantId { get; set; }

    public Guid PolicyId { get; set; }

    public DateOnly IncidentDate { get; set; }

    public string IncidentDescription { get; set; } = null!;

    public ClaimStatus Status { get; set; }

    public decimal EstimatedLiability { get; set; }

    public decimal? ApprovedAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? AssignedOfficerId { get; set; }
    public int Version { get; set; }
}
