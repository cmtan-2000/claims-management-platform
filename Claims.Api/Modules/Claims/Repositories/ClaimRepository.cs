using Claims.Api.Infrastructure.Persistence;
using Claims.Api.Modules.Claims.Entities;
using Claims.Api.Modules.Claims.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Modules.Claims.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly AppDbContext _db;

        public ClaimRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Claim claim)
        {
            await _db.Claims.AddAsync(claim);
        }

        public async Task<List<Claim>> GetAllAsync()
        {
            return await _db.Claims.AsNoTracking().ToListAsync();
        }

        public async Task<Claim?> GetByIdAsync(Guid claimId)
        {
            return await _db.Claims.FirstOrDefaultAsync(c => c.Id == claimId);
        }
    }
}