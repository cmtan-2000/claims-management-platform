using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Infrastructure.Persistence;
using Claims.Api.Modules.Claims.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Modules.Claims.Repositories
{
    public class ClaimantRepository : IClaimantRepository
    {
        private readonly AppDbContext _db;

        public ClaimantRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Claimant?> GetByIdAsync(Guid id)
        {
            return await _db.Claimants
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Claimant claimant)
        {
            await _db.Claimants.AddAsync(claimant);
        }

        public async Task<List<Claimant>> GetAllAsync()
        {
            return await _db.Claimants.AsNoTracking().ToListAsync();
        }
    }
}