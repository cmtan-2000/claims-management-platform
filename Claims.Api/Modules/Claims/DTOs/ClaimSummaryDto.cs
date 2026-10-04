using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class ClaimSummaryDto
    {
        public int Submitted { get; set; }
        public int UnderReview { get; set; }
        public int PendingSettlement { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
    }
}