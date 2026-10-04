using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Entities;
using Claims.Api.Modules.Claims.Interface.Repository;
using Claims.Api.Modules.Dashboards.DTOs;
using Claims.Api.Modules.Dashboards.Interface.Services;

namespace Claims.Api.Modules.Dashboards.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ISettlementRepository _settlementRepository;


        public DashboardService(IClaimRepository claimRepository, IAssessmentRepository assessmentRepository, ISettlementRepository settlementRepository)
        {
            _claimRepository = claimRepository;
            _assessmentRepository = assessmentRepository;
            _settlementRepository = settlementRepository;
        }

        public async Task<DashboardDto> GetDashboardAsync(Guid officerId)
        {
            var claims = await _claimRepository.GetAllAsync();
            var assessments = await _assessmentRepository.GetAllAsync();
            var settlements = await _settlementRepository.GetAllAsync();

            return new DashboardDto
            {
                Summary = new DashboardSummaryDto
                {
                    TotalClaims = claims.Count,

                    Submitted = claims.Count(
                        x => x.Status == ClaimStatus.Submitted),

                    UnderReview = claims.Count(
                        x => x.Status == ClaimStatus.UnderReview),

                    PendingSettlement = claims.Count(
                        x => x.Status == ClaimStatus.PendingSettlement),

                    Approved = claims.Count(
                        x => x.Status == ClaimStatus.Approved),

                    Rejected = claims.Count(
                        x => x.Status == ClaimStatus.Rejected)
                },

                Financial = new DashboardFinancialDto
                {
                    TotalEstimatedLoss =
                        claims.Sum(x => x.EstimatedLiability),

                    TotalApprovedAmount =
                        assessments.Sum(x => x.ApprovedAmount ?? 0),

                    TotalSettlementAmount =
                        settlements.Sum(x => x.Amount),

                    OutstandingLiability =
                        assessments.Sum(x => x.ApprovedAmount ?? 0)
                        - settlements.Sum(x => x.Amount)
                },

                OfficerWorkload = new OfficerWorkloadDto
                {
                    UnassignedClaims = claims.Count(
                        x => x.AssignedOfficerId == null),

                    MyActiveClaims = claims.Count(
                        x => x.AssignedOfficerId == officerId &&
                             x.Status == ClaimStatus.UnderReview),

                    PendingAssessment = claims.Count(
                        x => x.Status == ClaimStatus.PendingAssessment),

                    PendingSettlement = claims.Count(
                        x => x.Status == ClaimStatus.PendingSettlement)
                },

                RecentClaims = claims
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(10)
                    .Select(x => new RecentClaimDto
                    {
                        Id = x.Id,
                        ClaimNumber = x.ClaimNumber,
                        Status = x.Status,
                        IncidentDate = x.IncidentDate,
                        CreatedAt = x.CreatedAt
                    })
                    .ToList()
            };
        }
    }
}