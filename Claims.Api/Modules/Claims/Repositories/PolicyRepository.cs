using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Infrastructure.Persistence;
using Claims.Api.Modules.Claims.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Modules.Claims.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly AppDbContext _db;

        public PolicyRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Policy policy)
        {
            await _db.Policies.AddAsync(policy);
        }

        public async Task<List<Policy>> GetListByClaimantIdAsync(Guid claimantId)
        {
            return await _db.Policies.Where(p => p.ClaimantId == claimantId)
                .ToListAsync();
        }
    }
}