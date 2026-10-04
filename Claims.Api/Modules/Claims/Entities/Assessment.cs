using Claims.Api.Modules.Claims.Enum;

public class Assessment
{
    public Guid Id { get; set; }

    public Guid ClaimId { get; set; }

    public string AssessmentNotes { get; set; } = null!;

    public decimal EstimatedLoss { get; set; }

    public decimal? ApprovedAmount { get; set; }

    public AssessmentDecision Decision { get; set; }

    public DateTime CreatedAt { get; set; }
}