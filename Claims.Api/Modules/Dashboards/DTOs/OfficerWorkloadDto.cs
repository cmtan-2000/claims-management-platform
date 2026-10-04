using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Dashboards.DTOs
{
    public class OfficerWorkloadDto
    {
        public int UnassignedClaims { get; set; }

        public int MyActiveClaims { get; set; }

        public int PendingAssessment { get; set; }

        public int PendingSettlement { get; set; }
    }
}