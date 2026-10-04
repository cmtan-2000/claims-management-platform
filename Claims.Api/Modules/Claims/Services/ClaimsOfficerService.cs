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
    public class ClaimsOfficerService : IClaimsOfficerService
    {
        private readonly IClaimsOfficerRepository _claimsOfficerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClaimsOfficerService(
            IClaimsOfficerRepository claimsOfficerRepository,
            IUnitOfWork unitOfWork)
        {
            _claimsOfficerRepository = claimsOfficerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateAsync(CreateClaimsOfficerDto dto)
        {
            var officer = new ClaimsOfficer
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Team = dto.Team
            };

            await _claimsOfficerRepository.AddAsync(officer);

            await _unitOfWork.SaveChangesAsync();

            return officer.Id;
        }

        public async Task<List<ClaimsOfficer>> GetAllAsync()
        {
            return await _claimsOfficerRepository.GetAllAsync();
        }

        public async Task<ClaimsOfficer?> GetByEmailAsync(string email)
        {
            return await _claimsOfficerRepository.GetByEmailAsync(email.Trim().ToLowerInvariant());
        }

        public async Task<ClaimsOfficer?> GetByIdAsync(Guid id)
        {
            return await _claimsOfficerRepository.GetByIdAsync(id);
        }

    }
}