using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.Interface.Repository
{
    public interface IClaimsOfficerRepository
    {
        Task<ClaimsOfficer?> GetByIdAsync(Guid officerId);
        Task<ClaimsOfficer?> GetByEmailAsync(string email);
        Task<List<ClaimsOfficer>> GetAllAsync();
        Task AddAsync(ClaimsOfficer officer);

    }
}