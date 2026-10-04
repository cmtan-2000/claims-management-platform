using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;

namespace Claims.Api.Modules.Claims.Interface.Service
{
    public interface IClaimantService
    {
        Task<Guid> CreateAsync(CreateClaimantDto dto);
        Task<Claimant?> GetByIdAsync(Guid id);
        Task<List<Claimant>> GetAllAsync();
    }
}