using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Entities;
using Claims.Api.Modules.Claims.Interface;
using Claims.Api.Modules.Claims.Interface.Repository;
using Claims.Api.Modules.Claims.Interface.Service;
using Microsoft.EntityFrameworkCore;
using Claims.Api.Exceptions;

namespace Claims.Api.Modules.Claims.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepo;
        private readonly IClaimsOfficerRepository _officerRepo;
        private readonly IClaimStatusHistoryRepository _claimStatusHistoryRepo;
        private readonly IClaimantRepository _claimantRepo;
        private readonly ISettlementRepository _settlementRepo;
        private readonly IAssessmentRepository _assessmentRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ClaimService(IClaimRepository claimRepository, IClaimsOfficerRepository claimsOfficerRepository, IClaimStatusHistoryRepository claimStatusHistoryRepository, IUnitOfWork unitOfWork, IClaimantRepository claimantRepository, ISettlementRepository settlementRepository, IAssessmentRepository assessmentRepository)
        {
            _claimRepo = claimRepository;
            _officerRepo = claimsOfficerRepository;
            _claimStatusHistoryRepo = claimStatusHistoryRepository;
            _claimantRepo = claimantRepository;
            _settlementRepo = settlementRepository;
            _assessmentRepo = assessmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateAsync(CreateClaimDto dto)
        {
            var claimant = await _claimantRepo
                .GetByIdAsync(dto.ClaimantId);

            if (claimant is null)
                throw new NotFoundException("Claimant not found.");

            var claim = new Claim
            {
                Id = Guid.NewGuid(),
                ClaimNumber = dto.ClaimNumber,
                ClaimantId = dto.ClaimantId,
                PolicyId = dto.PolicyId,
                IncidentDate = dto.IncidentDate,
                IncidentDescription = dto.IncidentDescription,
                Status = ClaimStatus.Submitted,
                EstimatedLiability = dto.EstimatedLiability,
                CreatedAt = DateTime.UtcNow
            };

            await _claimRepo.AddAsync(claim);

            await _unitOfWork.SaveChangesAsync();

            return claim.Id;
        }

        public async Task<List<Claim>> GetAllAsync()
        {
            return await _claimRepo.GetAllAsync();
        }

        public async Task<Claim?> GetClaimByIdAsync(Guid id)
        {
            return await _claimRepo.GetByIdAsync(id);
        }

        public async Task<List<ClaimStatusHistory>> GetHistoryByClaimIdAsync(Guid claimId)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);

            if (claim is null)
                throw new NotFoundException("Claim not found.");

            return await _claimStatusHistoryRepo
                .GetByClaimIdAsync(claimId);
        }

        public async Task PickupClaimAsync(
            Guid claimId,
            Guid officerId)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);

            if (claim is null)
            {
                throw new KeyNotFoundException("Claim not found.");
            }

            var officer = await _officerRepo.GetByIdAsync(officerId);

            if (officer is null)
                throw new KeyNotFoundException("Officer not found.");

            if (claim.AssignedOfficerId is not null)
            {
                throw new InvalidOperationException("Claim has already been assigned.");
            }

            if (claim.Status != ClaimStatus.Submitted)
            {
                throw new InvalidOperationException("Claim cannot be picked up in its current status.");
            }

            var previousStatus = claim.Status;

            claim.AssignedOfficerId = officerId;
            claim.Status = ClaimStatus.UnderReview;

            var history = new ClaimStatusHistory
            {
                Id = Guid.NewGuid(),
                ClaimId = claim.Id,
                FromStatus = previousStatus,
                ToStatus = claim.Status,
                ChangedByOfficerId = officerId,
                ChangedAt = DateTime.UtcNow,
                Reason = "Claim picked up"
            };

            await _claimStatusHistoryRepo.AddAsync(history);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("Claim was already updated by another officer.");
            }
        }

        public async Task<Claim> AddStatusHistoryAsync(Guid claimId, Guid officerId, UpdateClaimStatusDto dto)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);

            if (claim is null)
                throw new NotFoundException("Claim not found.");

            var oldStatus = claim.Status;

            claim.Status = dto.Status;

            var history = new ClaimStatusHistory
            {
                Id = Guid.NewGuid(),
                ClaimId = claim.Id,
                FromStatus = oldStatus,
                ToStatus = dto.Status,
                ChangedByOfficerId = officerId,
                ChangedAt = DateTime.UtcNow,
                Reason = dto.Reason
            };

            await _claimStatusHistoryRepo.AddAsync(history);

            await _unitOfWork.SaveChangesAsync();

            return new Claim();
        }

        public async Task<Claim?> UpdateAsync(Guid claimId, ClaimStatus claimStatus)
        {
            var claim = await _claimRepo.GetByIdAsync(claimId);
            if (claim is null)
                throw new NotFoundException("Claim not found.");

            claim.Status = claimStatus;
            claim.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            return claim;
        }

        public async Task<List<ClaimListDto>> GetMyClaimsAsync(Guid claimantId)
        {
            var claims = await _claimRepo.GetAllAsync();

            return claims
                .Where(x => x.ClaimantId == claimantId)
                .Select(x => new ClaimListDto
                {
                    Id = x.Id,
                    ClaimNumber = x.ClaimNumber,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt
                })
                .ToList();
        }

        public async Task<List<OfficerClaimListDto>> GetAllClaimsForOfficerAsync()
        {
            var claims = await _claimRepo.GetAllAsync();

            return claims
                .Select(x => new OfficerClaimListDto
                {
                    Id = x.Id,
                    ClaimNumber = x.ClaimNumber,
                    ClaimantId = x.ClaimantId,
                    AssignedOfficerId = x.AssignedOfficerId,
                    Status = x.Status,
                    EstimatedLiability = x.EstimatedLiability,
                    CreatedAt = x.CreatedAt
                })
                .ToList();
        }

        public async Task<ClaimSummaryDto> GetClaimSummaryAsync()
        {
            var claims = await _claimRepo.GetAllAsync();

            return new ClaimSummaryDto
            {
                Submitted = claims.Count(x =>
                    x.Status == ClaimStatus.Submitted),

                UnderReview = claims.Count(x =>
                    x.Status == ClaimStatus.UnderReview),

                PendingSettlement = claims.Count(x =>
                    x.Status == ClaimStatus.PendingSettlement),

                Approved = claims.Count(x =>
                    x.Status == ClaimStatus.Approved),

                Rejected = claims.Count(x =>
                    x.Status == ClaimStatus.Rejected)
            };
        }

        public async Task<LiabilityDto> GetLiabilityAsync()
        {
            var claims = await _claimRepo.GetAllAsync();
            var assessments = await _assessmentRepo.GetAllAsync();
            var settlements = await _settlementRepo.GetAllAsync();

            var totalEstimatedLoss = claims
                .Sum(x => x.EstimatedLiability);

            var totalApprovedAmount = assessments
                .Sum(x => x.ApprovedAmount ?? 0);

            var totalSettlementAmount = settlements
                .Sum(x => x.Amount);

            return new LiabilityDto
            {
                TotalEstimatedLoss = totalEstimatedLoss,
                TotalApprovedAmount = totalApprovedAmount,
                TotalSettlementAmount = totalSettlementAmount,
                OutstandingLiability =
                    totalApprovedAmount - totalSettlementAmount
            };
        }
    }
}