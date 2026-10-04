using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.Interface.Repository
{
    public interface IPolicyRepository
    {
        Task<Policy?> GetByIdAsync(Guid id);
        Task AddAsync(Policy policy);
    }
}