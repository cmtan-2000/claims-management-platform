using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Infrastructure.Persistence;
using Claims.Api.Modules.Claims.Interface;
using Claims.Api.Modules.Claims.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Modules.Claims.Repositories
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly AppDbContext _db;

        public AssessmentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Assessment assessment)
        {
            await _db.Assessments.AddAsync(assessment);
        }

        public async Task<List<Assessment>> GetAllAsync()
        {
            return await _db.Assessments.AsNoTracking().ToListAsync();
        }

        public async Task<Assessment?> GetByIdAsync(Guid id)
        {
            return await _db.Assessments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Assessment?> GetByClaimIdAsync(Guid claimId)
        {
            return await _db.Assessments.FirstOrDefaultAsync(x => x.ClaimId == claimId);
        }
    }
}