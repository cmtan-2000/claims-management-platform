using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;

namespace Claims.Api.Modules.Claims.Interface.Service
{
    public interface IAssessmentService
    {
        Task<Guid> CreateAsync(Guid claimId, CreateAssessmentDto dto);
        Task<Assessment> UpdateAsync(Guid claimId, UpdateAssessmentDto dto);
        Task<Assessment?> GetByClaimIdAsync(Guid claimId);
        Task<List<Assessment>> GetAllAsync();
    }
}