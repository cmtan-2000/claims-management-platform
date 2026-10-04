using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Claims.Api.Infrastructure.Persistence;
using Claims.Api.Modules.Claims.Entities;
using Claims.Api.Modules.Claims.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Modules.Claims.Repositories
{
    public class ClaimStatusHistoryRepository : IClaimStatusHistoryRepository
    {
        private AppDbContext _db;

        public ClaimStatusHistoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(ClaimStatusHistory history)
        {
            await _db.ClaimStatusHistories.AddAsync(history);
        }

        public async Task<List<ClaimStatusHistory>> GetByClaimIdAsync(Guid claimId)
        {
            return await _db.ClaimStatusHistories
                .Where(x => x.ClaimId == claimId)
                .OrderByDescending(x => x.ChangedAt)
                .ToListAsync();
        }
    }
}