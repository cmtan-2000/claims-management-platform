public class Settlement
{
    public Guid Id { get; set; }

    public Guid ClaimId { get; set; }

    public decimal Amount { get; set; }

    public DateTime SettledAt { get; set; }

    public string Reference { get; set; } = null!;
}