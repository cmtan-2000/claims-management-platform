using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Infrastructure.Persistence;
using Claims.Api.Modules.Claims.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Modules.Claims.Repositories
{
    public class ClaimsOfficerRepository : IClaimsOfficerRepository
    {
        private readonly AppDbContext _db;

        public ClaimsOfficerRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(ClaimsOfficer officer)
        {
            await _db.ClaimsOfficers.AddAsync(officer);
        }

        public async Task<List<ClaimsOfficer>> GetAllAsync()
        {
            return await _db.ClaimsOfficers.AsNoTracking().ToListAsync();
        }

        public async Task<ClaimsOfficer?> GetByEmailAsync(string email)
        {
            email = email.Trim().ToLowerInvariant();

            return await _db.ClaimsOfficers
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email);
        }

        public async Task<ClaimsOfficer?> GetByIdAsync(Guid officerId)
        {
            return await _db.ClaimsOfficers.FirstOrDefaultAsync(o => o.Id == officerId);
        }
    }
}