using System.Security.Claims;
using Claims.Api.Exceptions;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Entities;
using Claims.Api.Modules.Claims.Enum;
using Claims.Api.Modules.Claims.Interface;
using Claims.Api.Modules.Claims.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Modules.Claims.Controllers
{
    [ApiController]
    [Route("api/claims")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;
        private readonly IInformationRequestService _informationRequestService;
        private readonly ISettlementService _settlementService;
        private readonly IAssessmentService _assessmentService;


        public ClaimsController(IClaimService claimService, IInformationRequestService informationRequestService, IAssessmentService assessmentService, ISettlementService settlementService)
        {
            _claimService = claimService;
            _informationRequestService = informationRequestService;
            _assessmentService = assessmentService;
            _settlementService = settlementService;
        }

        [HttpPost()]
        [Authorize(Roles = "Claimant")]
        public async Task<IActionResult> SubmitClaim([FromBody] CreateClaimDto dto)
        {
            var claimantIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(claimantIdValue, out var claimantId))
            {
                return Unauthorized();
            }

            dto.ClaimantId = claimantId;
            var result = await _claimService.CreateAsync(dto);

            return Ok(new
            {
                id = result
            });
        }

        [HttpGet]
        [Authorize(Roles = "Claimant,ClaimsOfficer")]
        public async Task<IActionResult> GetAllClaims()
        {
            var userIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var role =
                User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "Claimant")
            {
                var claimantClaims =
                    await _claimService.GetMyClaimsAsync(userId);

                return Ok(claimantClaims);
            }

            if (role == "ClaimsOfficer")
            {
                var officerClaims =
                    await _claimService.GetAllClaimsForOfficerAsync();

                return Ok(officerClaims);
            }

            return Forbid();
        }

        [HttpGet("{claimId}")]
        // [Authorize(Roles = "Claimant")]
        public async Task<IActionResult> GetClaim(Guid claimId)
        {
            // var claimantIdValue =
            //     User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // if (!Guid.TryParse(claimantIdValue, out var claimantId))
            // {
            //     return Unauthorized();
            // }

            var result = await _claimService.GetClaimByIdAsync(claimId);

            return Ok(result);
        }

        [HttpPost("{claimId}/pickup")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> PickupClaim(Guid claimId)
        {
            var officerIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(officerIdValue, out var officerId))
            {
                return Unauthorized();
            }

            await _claimService.PickupClaimAsync(
                claimId,
                officerId);

            return NoContent();
        }

        [HttpPatch("{claimId}/status")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> UpdateStatusHistory(Guid claimId, [FromBody] UpdateClaimStatusDto dto)
        {
            var officerIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(officerIdValue, out var officerId))
                return Unauthorized();

            await _claimService.AddStatusHistoryAsync(
                claimId,
                officerId,
                dto);

            return Ok(new
            {
                claimId,
                status = dto.Status
            });
        }

        [HttpGet("{claimId}/status-history")]
        [Authorize]
        public async Task<IActionResult> GetStatusHistory(Guid claimId)
        {
            var history =
                await _claimService.GetHistoryByClaimIdAsync(claimId);

            return Ok(new
            {
                items = history
            });
        }

        [HttpGet("{claimId}/information-requests")]
        // [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> GetInformationRequests(Guid claimId)
        {
            var result =
                await _informationRequestService
                    .GetByClaimIdAsync(claimId);

            return Ok(result);
        }

        [HttpPost("{claimId}/information-requests")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> CreateInformationRequest(Guid claimId,
            [FromBody] CreateInformationRequestDto dto)
        {
            var officerIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(officerIdValue, out var officerId))
                return Unauthorized();

            var informationResult = await _informationRequestService.CreateAsync(claimId, officerId, dto);

            var claimStatusDto = new UpdateClaimStatusDto
            {
                Status = ClaimStatus.AwaitingInformation,
                Reason = "Information Requested, awaiting information"
            };

            await _claimService.AddStatusHistoryAsync(claimId, officerId, claimStatusDto);

            var claimResult = await _claimService.UpdateAsync(claimId, ClaimStatus.AwaitingInformation);

            return Ok(new
            {
                information = informationResult,
                claim = claimResult
            });
        }

        [HttpPost("information-requests/{requestId}/response")]
        [Authorize(Roles = "Claimant")]
        public async Task<IActionResult> SubmitInformationRequest(Guid requestId,
            [FromBody] UpdateInformationRequestDto dto)
        {
            var claimantIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(claimantIdValue, out var claimantId))
                return Unauthorized();

            var informationResult = await _informationRequestService.UpdateResponseRequestAsync(requestId, dto);


            // UnderAssessment
            var claimStatusDto = new UpdateClaimStatusDto
            {
                Status = ClaimStatus.PendingAssessment,
                Reason = "Information Resubmitted, Undergoing Assessment"
            };

            await _claimService.AddStatusHistoryAsync(informationResult.ClaimId, informationResult.RequestedByOfficerId, claimStatusDto);

            var claimResult = await _claimService.UpdateAsync(informationResult.ClaimId, ClaimStatus.PendingAssessment);

            return Ok(new
            {
                information = informationResult,
                claim = claimResult
            });
        }

        [HttpPost("{claimId}/assessment")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> CreateAssessment(Guid claimId,
            [FromBody] CreateAssessmentDto dto)
        {
            var officerIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(officerIdValue, out var officerId))
                return Unauthorized();

            var claim = await _claimService.GetClaimByIdAsync(claimId);
            if (claim!.Status != ClaimStatus.UnderReview && claim!.Status != ClaimStatus.PendingAssessment)
                throw new InvalidOperationException("Claim Status Must be UnderReview or PendingAssessment");

            var result = await _assessmentService.CreateAsync(claimId, dto);

            return Ok(new
            {
                id = result
            });
        }

        [HttpPatch("{claimId}/assessment")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> UpdateAssessment(Guid claimId,
            [FromBody] UpdateAssessmentDto dto)
        {
            var officerIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(officerIdValue, out var officerId))
                return Unauthorized();

            var assessmentResult = await _assessmentService.UpdateAsync(claimId, dto);


            ClaimStatus claimStat = dto.Decision == AssessmentDecision.Approve
                ? ClaimStatus.PendingSettlement
                : ClaimStatus.Rejected;

            var claimStatusDto = new UpdateClaimStatusDto
            {
                Status = claimStat,
                Reason = dto.Decision == AssessmentDecision.Approve ? "Assessment Approved" : "Assessment Rejected"
            };

            await _claimService.AddStatusHistoryAsync(claimId, officerId, claimStatusDto);

            var claimResult = await _claimService.UpdateAsync(claimId, claimStat);

            return Ok(new
            {
                assessment = assessmentResult,
                claim = claimResult
            });
        }

        [HttpGet("assessment")]
        // [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> GetAllAssessment()
        {
            var result =
                await _assessmentService.GetAllAsync();

            return Ok(result);
        }

        [HttpPost("{claimId}/settlement")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> SubmitSettlement(Guid claimId, [FromBody] CreateSettlementDto dto)
        {
            var officerIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(officerIdValue, out var officerId))
                return Unauthorized();

            //  Check Claim Exist
            var claim = await _claimService.GetClaimByIdAsync(claimId);
            if (claim is null)
                throw new NotFoundException("Claim not found.");

            //  Assigned Officer
            if (claim.AssignedOfficerId != officerId)
                throw new InvalidOperationException("Officer Mismatched");

            //  Claim Status = Pending Settlement
            if (claim.Status != ClaimStatus.PendingSettlement)
                throw new InvalidOperationException("Status Invalid");

            //  Assessment Exist
            var assessment = await _assessmentService.GetByClaimIdAsync(claim.Id);
            if (assessment is null)
                throw new NotFoundException("Assessment not found.");

            //  Assessment Approved?
            if (assessment.Decision != AssessmentDecision.Approve)
                throw new InvalidOperationException("Decision Assessment Invalid");

            //  Assessment Approved?
            if (dto.SettlementAmount > assessment.ApprovedAmount)
                throw new InvalidOperationException("Settlement Amount must within Approved Amount");

            var settlement = new Settlement
            {
                Id = new Guid(),
                ClaimId = claim.Id,
                Amount = dto.SettlementAmount,
                SettledAt = DateTime.UtcNow,
                Reference = dto.Reference
            };
            var result = await _settlementService.CreateAsync(claimId, settlement);

            // 
            var claimStatusDto = new UpdateClaimStatusDto
            {
                Status = ClaimStatus.Approved,
                Reason = "Settlement Completed"
            };

            await _claimService.AddStatusHistoryAsync(claimId, officerId, claimStatusDto);

            var claimResult = await _claimService.UpdateAsync(claimId, ClaimStatus.Approved);

            return Ok(new
            {
                id = result
            });
        }

        [HttpGet("settlement")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> GetAllSettlement()
        {
            var result =
                await _settlementService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("summary")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> GetClaimSummary()
        {
            var result = await _claimService.GetClaimSummaryAsync();

            return Ok(result);
        }

        [HttpGet("liability")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> GetLiability()
        {
            var result = await _claimService.GetLiabilityAsync();

            return Ok(result);
        }
    }
}