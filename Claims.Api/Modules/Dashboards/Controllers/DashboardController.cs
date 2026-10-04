using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Claims.Api.Modules.Dashboards.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Modules.Dashboards.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> GetDashboard()
        {
            var userIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdValue, out var officerId))
            {
                return Unauthorized();
            }

            var result = await _dashboardService
                .GetDashboardAsync(officerId);

            return Ok(result);
        }
    }
}