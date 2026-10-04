using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;

namespace Claims.Api.Modules.Claims.Interface.Service
{
    public interface IPolicyService
    {
        Task<Guid> CreateAsync(Guid claimantId, CreatePolicyDto dto);
        Task<List<Policy>> GetListByClaimantIdAsync(Guid claimantId);

    }
}