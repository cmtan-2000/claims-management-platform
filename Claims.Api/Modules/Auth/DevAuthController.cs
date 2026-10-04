using Claims.Api.Modules.Auth;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Interface.Service;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/dev-auth")]
public class DevAuthController : ControllerBase
{
    private readonly JwtTokenGenerator _tokenGenerator;
    private readonly IClaimantService _claimantService;
    private readonly AuthService _authService;
    public DevAuthController(JwtTokenGenerator tokenGenerator, IClaimantService claimantService, AuthService authService)
    {
        _tokenGenerator = tokenGenerator;
        _claimantService = claimantService;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email."
            });
        }

        return Ok(result);
    }

    // [HttpGet("token/{role}")]
    // public async Task<IActionResult> GenerateToken(string role, [FromQuery] Guid userId)
    // {

    //     var userRole = role == "claimsofficer" ? "ClaimsOfficer" :
    //         role == "claimant" ? "Claimant" : "Invalid";

    //     if (userRole == "Claimant")
    //     {
    //         var user = await _claimantService.GetByIdAsync(userId);
    //         if (user is null) return NotFound("Claimant not found.");
    //     }
    //     else
    //     {
    //         // var user = await claimsOfficerSerbive.GetByIdAsync(userId);
    //         // if (user is null) return NotFound("Claims Officer not found.");
    //     }

    //     if (userRole == "Invalid") return BadRequest("Invalid Role");

    //     var token = _tokenGenerator.Generate(userId, userRole);

    //     return Ok(new
    //     {
    //         token
    //     });
    // }
}