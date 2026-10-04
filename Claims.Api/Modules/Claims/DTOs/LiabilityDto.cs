using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class LiabilityDto
    {
        public decimal TotalEstimatedLoss { get; set; }

        public decimal TotalApprovedAmount { get; set; }

        public decimal TotalSettlementAmount { get; set; }

        public decimal OutstandingLiability { get; set; }
    }
}