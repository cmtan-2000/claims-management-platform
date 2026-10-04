using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.Interface.Repository
{
    public interface IClaimantRepository
    {
        Task<Claimant?> GetByIdAsync(Guid id);
        Task<Claimant?> GetByEmailAsync(string email);

        Task<List<Claimant>> GetAllAsync();

        Task AddAsync(Claimant claimant);
    }
}