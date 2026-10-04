public class InformationRequest
{
    public Guid Id { get; set; }

    public Guid ClaimId { get; set; }

    public Guid RequestedByOfficerId { get; set; }

    public string Question { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Response { get; set; }

    public string Status { get; set; } = null!;

    public DateTime RequestedAt { get; set; }

    public DateTime? RespondedAt { get; set; }
}