using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Entities;

namespace Claims.Api.Modules.Claims.Interface.Service
{
    public interface IClaimService
    {
        Task PickupClaimAsync(Guid claimId, Guid officerId);
        Task<Guid> CreateAsync(CreateClaimDto dto);
        Task<Claim?> GetClaimByIdAsync(Guid id);
        Task<List<Claim>> GetAllAsync();
        Task<Claim?> UpdateAsync(Guid claimId, ClaimStatus status);
        Task<Claim> AddStatusHistoryAsync(Guid claimdId, Guid officerId, UpdateClaimStatusDto dto);
        Task<List<ClaimStatusHistory>> GetHistoryByClaimIdAsync(Guid claimId);
        Task<List<ClaimListDto>> GetMyClaimsAsync(Guid claimantId);
        Task<List<OfficerClaimListDto>> GetAllClaimsForOfficerAsync();
        Task<ClaimSummaryDto> GetClaimSummaryAsync();
        Task<LiabilityDto> GetLiabilityAsync();
    }
}