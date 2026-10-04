using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;

namespace Claims.Api.Modules.Claims.Interface.Service
{
    public interface ISettlementService
    {
        Task<Guid> CreateAsync(Guid claimId, Settlement settlement);
        Task<List<Settlement>> GetAllAsync();
    }
}