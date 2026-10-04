using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Claims.Api.Exceptions;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Enum;
using Claims.Api.Modules.Claims.Interface;
using Claims.Api.Modules.Claims.Interface.Repository;
using Claims.Api.Modules.Claims.Interface.Service;

namespace Claims.Api.Modules.Claims.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssessmentService(IAssessmentRepository assessmentRepository, IUnitOfWork unitOfWork)
        {
            _assessmentRepository = assessmentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> CreateAsync(Guid claimId, CreateAssessmentDto dto)
        {
            await _assessmentRepository.GetByClaimIdAsync(claimId);

            var assessment = new Assessment()
            {
                ClaimId = claimId,
                AssessmentNotes = dto.AssessmentNotes,
                EstimatedLoss = dto.EstimatedLoss,
                CreatedAt = DateTime.UtcNow
            };

            await _assessmentRepository.AddAsync(assessment);
            await _unitOfWork.SaveChangesAsync();
            return assessment.Id;
        }

        public async Task<Assessment> UpdateAsync(Guid claimId, UpdateAssessmentDto dto)
        {
            var assessment = await _assessmentRepository.GetByIdAsync(dto.AssessmentId);

            if (assessment is null)
                throw new NotFoundException("Assessment not found.");

            if (dto.Decision == AssessmentDecision.Approve)
            {
                assessment.ApprovedAmount = dto.ApprovedAmount;
            }
            else if (dto.Decision == AssessmentDecision.Reject)
            {
                assessment.ApprovedAmount = null;
            }
            assessment.Decision = dto.Decision;

            await _unitOfWork.SaveChangesAsync();

            return assessment;
        }

        public Task<List<Assessment>> GetAllAsync()
        {
            return _assessmentRepository.GetAllAsync();
        }

        public Task<Assessment?> GetByClaimIdAsync(Guid claimId)
        {
            return _assessmentRepository.GetByClaimIdAsync(claimId);
        }
    }
}