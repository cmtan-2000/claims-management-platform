using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Dashboards.DTOs;

namespace Claims.Api.Modules.Dashboards.Interface.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardAsync(Guid officerId);
    }
}