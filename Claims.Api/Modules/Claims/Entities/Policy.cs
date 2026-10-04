using Claims.Api.Modules.Claims.Enum;

public class Policy
{
    public Guid Id { get; set; }
    public Guid ClaimantId { get; set; }
    public string PolicyNumber { get; set; } = null!;
    public PolicyType PolicyType { get; set; }
    public Market Market { get; set; }
}