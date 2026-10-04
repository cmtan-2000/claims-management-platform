using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Modules.Claims.Controllers
{
    [ApiController]
    [Route("api/claimants")]
    public class ClaimantController : ControllerBase
    {
        private readonly IClaimantService _claimantService;

        public ClaimantController(IClaimantService claimantService)
        {
            _claimantService = claimantService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaimant(
            CreateClaimantDto dto)
        {
            var claimantId =
                await _claimantService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = claimantId },
                new { id = claimantId });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var claimant =
                await _claimantService.GetByIdAsync(id);

            if (claimant is null)
                return NotFound();

            return Ok(claimant);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var claimant =
                await _claimantService.GetAllAsync();

            if (claimant is null)
                return NotFound();

            return Ok(claimant);
        }
    }
}