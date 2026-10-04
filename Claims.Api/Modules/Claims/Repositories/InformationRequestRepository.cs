using Claims.Api.Infrastructure.Persistence;
using Claims.Api.Modules.Claims.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Modules.Claims.Repositories
{
    public class InformationRequestRepository : IInformationRequestRepository
    {
        private readonly AppDbContext _db;

        public InformationRequestRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(InformationRequest request)
        {
            await _db.InformationRequests.AddAsync(request);
        }

        public async Task<List<InformationRequest>> GetByClaimIdAsync(Guid claimId)
        {
            return await _db.InformationRequests
                .Where(x => x.ClaimId == claimId)
                .OrderByDescending(x => x.RequestedAt)
                .ToListAsync();
        }

        public async Task<InformationRequest?> GetByIdAsync(Guid requestId)
        {
            return await _db.InformationRequests
                .FirstOrDefaultAsync(ir => ir.Id == requestId);
        }
    }
}