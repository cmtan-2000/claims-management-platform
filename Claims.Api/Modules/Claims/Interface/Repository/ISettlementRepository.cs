using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.Interface.Repository
{
    public interface ISettlementRepository
    {
        Task<Settlement?> GetByIdAsync(Guid id);
        Task<List<Settlement>> GetAllAsync();
        Task AddAsync(Settlement settlement);
    }
}