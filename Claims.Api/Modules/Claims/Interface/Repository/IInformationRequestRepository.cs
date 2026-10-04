using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.Interface.Repository
{
    public interface IInformationRequestRepository
    {
        Task<List<InformationRequest>> GetByClaimIdAsync(Guid claimId);
        Task<InformationRequest?> GetByIdAsync(Guid requestId);

        Task AddAsync(InformationRequest request);
    }
}