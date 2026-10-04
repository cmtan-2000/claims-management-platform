using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;

namespace Claims.Api.Modules.Claims.Interface.Service
{
    public interface IInformationRequestService
    {
        Task<List<InformationRequest>> GetByClaimIdAsync(Guid claimId);
        Task<InformationRequest> CreateAsync(Guid claimId, Guid officerId, CreateInformationRequestDto ir);
        Task<InformationRequest> UpdateResponseRequestAsync(Guid requestId, UpdateInformationRequestDto ir);

    }
}