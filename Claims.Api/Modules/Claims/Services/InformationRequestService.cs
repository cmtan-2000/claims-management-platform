using Claims.Api.Modules.Claims.Interface;
using Claims.Api.Modules.Claims.Interface.Service;
using Claims.Api.Modules.Claims.Interface.Repository;
using Claims.Api.Exceptions;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Entities;
using Claims.Api.Modules.Claims.Enum;

namespace Claims.Api.Modules.Claims.Services
{
    public class InformationRequestService : IInformationRequestService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IInformationRequestRepository _informationRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InformationRequestService(IClaimRepository claimRepository, IInformationRequestRepository informationRequestRepository, IUnitOfWork unitOfWork)
        {
            _claimRepository = claimRepository;
            _informationRequestRepository = informationRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<InformationRequest> CreateAsync(Guid claimId, Guid officerId, CreateInformationRequestDto ir)
        {
            var claim = await _claimRepository.GetByIdAsync(claimId);

            if (claim is null)
                throw new NotFoundException("Claim not found.");

            var request = new InformationRequest
            {
                Id = Guid.NewGuid(),
                ClaimId = claimId,
                RequestedByOfficerId = officerId,
                Question = ir.Question,
                Description = ir.Description,
                Status = RequestStatus.Pending.ToString(),
                RequestedAt = DateTime.UtcNow
            };

            await _informationRequestRepository.AddAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return request;
        }

        public async Task<List<InformationRequest>> GetByClaimIdAsync(Guid claimId)
        {
            var claim = await _claimRepository.GetByIdAsync(claimId);

            if (claim is null)
                throw new NotFoundException("Claim not found.");

            return await _informationRequestRepository
                .GetByClaimIdAsync(claimId);
        }

        public async Task<InformationRequest> UpdateResponseRequestAsync(Guid requestId, UpdateInformationRequestDto ir)
        {
            var information = await _informationRequestRepository.GetByIdAsync(requestId);

            if (information is null)
                throw new NotFoundException("Information Request not found.");

            information.Response = ir.Response;
            information.Status = InformationRequestStatus.Responded.ToString();
            information.RespondedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return information;
        }
    }
}