using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.Interface.Repository
{
    public interface IAssessmentRepository
    {
        Task<Assessment?> GetByIdAsync(Guid id);
        Task<Assessment?> GetByClaimIdAsync(Guid claimId);
        Task<List<Assessment>> GetAllAsync();
        Task AddAsync(Assessment assessment);
    }
}