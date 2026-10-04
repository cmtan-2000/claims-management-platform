using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;

namespace Claims.Api.Modules.Claims.Interface.Service
{
    public interface IClaimsOfficerService
    {
        Task<Guid> CreateAsync(CreateClaimsOfficerDto dto);
        Task<ClaimsOfficer?> GetByIdAsync(Guid id);
        Task<List<ClaimsOfficer>> GetAllAsync();
    }
}