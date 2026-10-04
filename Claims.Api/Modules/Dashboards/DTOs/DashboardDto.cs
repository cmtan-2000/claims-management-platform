using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Dashboards.DTOs
{
    public class DashboardDto
    {
        public DashboardSummaryDto Summary { get; set; } = null!;

        public DashboardFinancialDto Financial { get; set; } = null!;

        public OfficerWorkloadDto OfficerWorkload { get; set; } = null!;

        public List<RecentClaimDto> RecentClaims { get; set; } = [];
    }
}