using Claims.Api.Modules.Claims.Interface.Service;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/dev-auth")]
public class DevAuthController : ControllerBase
{
    private readonly JwtTokenGenerator _tokenGenerator;
    private readonly IClaimantService _claimantService;
    public DevAuthController(JwtTokenGenerator tokenGenerator, IClaimantService claimantService)
    {
        _tokenGenerator = tokenGenerator;
        _claimantService = claimantService;
    }

    [HttpGet("token/{role}")]
    public async Task<IActionResult> GenerateToken(string role, [FromQuery] Guid userId)
    {

        var userRole = role == "claimsofficer" ? "ClaimsOfficer" :
            role == "claimant" ? "Claimant" : "Invalid";

        if (userRole == "Claimant")
        {
            var user = await _claimantService.GetByIdAsync(userId);
            if (user is null) return NotFound("Claimant not found.");
        }
        else
        {
            // var user = await claimsOfficerSerbive.GetByIdAsync(userId);
            // if (user is null) return NotFound("Claims Officer not found.");
        }

        if (userRole == "Invalid") return BadRequest("Invalid Role");

        var token = _tokenGenerator.Generate(userId, userRole);

        return Ok(new
        {
            token
        });
    }
}