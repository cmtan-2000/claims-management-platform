using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Entities;

namespace Claims.Api.Modules.Claims.Interface.Repository
{
    public interface IClaimStatusHistoryRepository
    {
        Task AddAsync(ClaimStatusHistory history);
        Task<List<ClaimStatusHistory>> GetByClaimIdAsync(Guid claimId);
    }
}