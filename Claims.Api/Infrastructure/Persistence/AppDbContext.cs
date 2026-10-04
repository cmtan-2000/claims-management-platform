using Claims.Api.Modules.Claims.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Claim> Claims => Set<Claim>();
    public DbSet<Claimant> Claimants => Set<Claimant>();
    public DbSet<ClaimsOfficer> ClaimsOfficers => Set<ClaimsOfficer>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<InformationRequest> InformationRequests => Set<InformationRequest>();
    public DbSet<Settlement> Settlements => Set<Settlement>();
    public DbSet<ClaimStatusHistory> ClaimStatusHistories => Set<ClaimStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // Relationships
        // =========================

        modelBuilder.Entity<Claim>()
            .HasOne<Claimant>()
            .WithMany()
            .HasForeignKey(c => c.ClaimantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Claim>()
            .HasOne<Policy>()
            .WithMany()
            .HasForeignKey(c => c.PolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Claim>()
            .HasOne<ClaimsOfficer>()
            .WithMany()
            .HasForeignKey(c => c.AssignedOfficerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Claim>()
            .HasMany<Assessment>()
            .WithOne()
            .HasForeignKey(a => a.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Claim>()
            .HasMany<InformationRequest>()
            .WithOne()
            .HasForeignKey(a => a.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Claim>()
            .HasOne<Settlement>()
            .WithOne()
            .HasForeignKey<Settlement>(a => a.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Claim>()
            .HasMany<ClaimStatusHistory>()
            .WithOne()
            .HasForeignKey(a => a.ClaimId)
            .OnDelete(DeleteBehavior.Restrict);

        // StatusHistory → Officer
        modelBuilder.Entity<ClaimStatusHistory>()
            .HasOne<ClaimsOfficer>()
            .WithMany()
            .HasForeignKey(h => h.ChangedByOfficerId)
            .OnDelete(DeleteBehavior.Restrict);

        // InformationRequest → Officer
        modelBuilder.Entity<InformationRequest>()
            .HasOne<ClaimsOfficer>()
            .WithMany()
            .HasForeignKey(r => r.RequestedByOfficerId)
            .OnDelete(DeleteBehavior.Restrict);

        //  Claimants -> Policy
        modelBuilder.Entity<Claimant>()
            .HasMany<Policy>()
            .WithOne()
            .HasForeignKey(p => p.ClaimantId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // Constraints
        // =========================

        modelBuilder.Entity<Claim>()
            .HasIndex(c => c.ClaimNumber)
            .IsUnique();

        modelBuilder.Entity<Policy>()
            .HasIndex(p => p.PolicyNumber)
            .IsUnique();

        modelBuilder.Entity<Settlement>()
            .HasIndex(s => s.ClaimId)
            .IsUnique();

        // =========================
        // Indexes
        // =========================

        modelBuilder.Entity<Claim>()
            .HasIndex(c => c.ClaimantId);

        modelBuilder.Entity<Claim>()
            .HasIndex(c => c.AssignedOfficerId);

        modelBuilder.Entity<Assessment>()
            .HasIndex(a => a.ClaimId);

        modelBuilder.Entity<InformationRequest>()
            .HasIndex(r => r.ClaimId);

        modelBuilder.Entity<ClaimStatusHistory>()
            .HasIndex(h => h.ClaimId);

        // =========================
        // Enum Mapping
        // =========================

        modelBuilder.Entity<Claim>()
            .Property(c => c.Status)
            .HasConversion<string>();

        modelBuilder.Entity<ClaimStatusHistory>()
            .Property(h => h.FromStatus)
            .HasConversion<string>();

        modelBuilder.Entity<ClaimStatusHistory>()
            .Property(h => h.ToStatus)
            .HasConversion<string>();

        modelBuilder.Entity<Policy>()
                .Property(p => p.PolicyType)
            .HasConversion<string>();

        modelBuilder.Entity<Policy>()
            .Property(p => p.Market)
            .HasConversion<string>();

        modelBuilder.Entity<Assessment>()
            .Property(p => p.Decision)
            .HasConversion<string>();

        // =========================
        // Concurrency Token
        // =========================
        modelBuilder.Entity<Claim>()
            .Property(c => c.Version)
            .IsConcurrencyToken();
    }
}