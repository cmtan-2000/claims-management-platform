using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Auth.DTOs;
using Claims.Api.Modules.Claims.DTOs;
using Claims.Api.Modules.Claims.Interface.Repository;
using Claims.Api.Modules.Claims.Interface.Service;
using Claims.Api.Modules.Claims.Services;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Modules.Auth
{
    public class AuthService : ControllerBase
    {
        private readonly IClaimantService _claimantService;
        private readonly IClaimsOfficerService _claimsOfficerService;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthService(IClaimantService claimantsService, IClaimsOfficerService claimsOfficerService, JwtTokenGenerator jwtTokenGenerator)
        {
            _claimantService = claimantsService;
            _claimsOfficerService = claimsOfficerService;
            _tokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponse?> LoginAsync(
    LoginRequest request)
        {
            var email = request.Email.Trim().ToLower();

            var claimant = await _claimantService
                .GetByEmailAsync(email);

            if (claimant != null)
            {
                return new LoginResponse
                {
                    Token = _tokenGenerator.Generate(
                        claimant.Id,
                        "Claimant"
                    )
                };
            }

            var officer = await _claimsOfficerService
                .GetByEmailAsync(email);

            if (officer != null)
            {
                return new LoginResponse
                {
                    Token = _tokenGenerator.Generate(
                        officer.Id,
                        "ClaimsOfficer"
                    )
                };
            }

            return null;
        }
    }
}