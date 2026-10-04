using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Infrastructure.Persistence;
using Claims.Api.Modules.Claims.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Modules.Claims.Repositories
{
    public class SettlementRepository : ISettlementRepository
    {
        private readonly AppDbContext _db;

        public SettlementRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Settlement settlement)
        {
            await _db.Settlements.AddAsync(settlement);
        }

        public async Task<List<Settlement>> GetAllAsync()
        {
            return await _db.Settlements.AsNoTracking().ToListAsync();
        }

        public async Task<Settlement?> GetByIdAsync(Guid id)
        {
            return await _db.Settlements.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}