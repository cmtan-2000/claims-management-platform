using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Interface.Service;

namespace Claims.Api.Modules.Claims.Controllers
{
    [ApiController]
    [Route("api/policies")]
    public class PolicyController : Controller
    {
        private readonly ILogger<PolicyController> _logger;
        private readonly IPolicyService _policyService;

        public PolicyController(ILogger<PolicyController> logger, IPolicyService policyService)
        {
            _logger = logger;
            _policyService = policyService;
        }

        [HttpPost]
        [Authorize(Roles = "Claimant")]
        public async Task<IActionResult> CreatePolicy([FromBody] CreatePolicyDto dto)
        {
            if (dto is null)
                return BadRequest("Request body is null.");

            var claimantIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(claimantIdValue, out var claimantId))
                return Unauthorized();

            var policyId =
                await _policyService.CreateAsync(
                    claimantId,
                    dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = policyId },
                new { id = policyId });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var claimant =
                await _policyService.GetByIdAsync(id);

            if (claimant is null)
                return NotFound();

            return Ok(claimant);
        }
    }
}