using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Interface;
using Claims.Api.Modules.Claims.Interface.Service;
using Claims.Api.Modules.Claims.Interface.Repository;


namespace Claims.Api.Modules.Claims.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PolicyService(IPolicyRepository policyRepository, IUnitOfWork unitOfWork)
        {
            _policyRepository = policyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateAsync(Guid claimantId, CreatePolicyDto dto)
        {

            var policy = new Policy
            {
                Id = Guid.NewGuid(),
                ClaimantId = claimantId,
                PolicyNumber = dto.PolicyNumber,
                PolicyType = dto.PolicyType,
                Market = dto.Market
            };

            await _policyRepository.AddAsync(policy);

            await _unitOfWork.SaveChangesAsync();

            return policy.Id;
        }

        public async Task<List<Policy>> GetListByClaimantIdAsync(Guid claimantId)
        {
            return await _policyRepository.GetListByClaimantIdAsync(claimantId);
        }
    }
}