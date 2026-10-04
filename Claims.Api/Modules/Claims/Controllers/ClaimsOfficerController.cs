using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Modules.Claims.Controllers
{
    [ApiController]
    [Route("api/officer")]
    public class ClaimsOfficerController : ControllerBase
    {
        private readonly IClaimsOfficerService _claimsOfficerService;

        public ClaimsOfficerController(IClaimsOfficerService claimsOfficerService)
        {
            _claimsOfficerService = claimsOfficerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaimant(
            CreateClaimsOfficerDto dto)
        {
            var claimantId =
                await _claimsOfficerService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = claimantId },
                new { id = claimantId });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var claimant =
                await _claimsOfficerService.GetByIdAsync(id);

            if (claimant is null)
                return NotFound();

            return Ok(claimant);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var claimant =
                await _claimsOfficerService.GetAllAsync();

            if (claimant is null)
                return NotFound();

            return Ok(claimant);
        }
    }
}