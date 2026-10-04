using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Entities;

namespace Claims.Api.Modules.Claims.Interface.Repository
{
    public interface IClaimRepository
    {
        Task<Claim?> GetByIdAsync(Guid claimId);
        Task<List<Claim>> GetAllAsync();
        Task AddAsync(Claim claim);
    }
}