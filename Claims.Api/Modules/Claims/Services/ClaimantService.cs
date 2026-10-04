using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Interface;
using Claims.Api.Modules.Claims.Interface.Repository;
using Claims.Api.Modules.Claims.Interface.Service;

namespace Claims.Api.Modules.Claims.Services
{
    public class ClaimantService : IClaimantService
    {
        private readonly IClaimantRepository _claimantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClaimantService(
            IClaimantRepository claimantRepository,
            IUnitOfWork unitOfWork)
        {
            _claimantRepository = claimantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateAsync(CreateClaimantDto dto)
        {
            var claimant = new Claimant
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            await _claimantRepository.AddAsync(claimant);

            await _unitOfWork.SaveChangesAsync();

            return claimant.Id;
        }

        public async Task<List<Claimant>> GetAllAsync()
        {
            return await _claimantRepository.GetAllAsync();
        }

        public async Task<Claimant?> GetByIdAsync(Guid id)
        {
            return await _claimantRepository.GetByIdAsync(id);
        }

    }
}