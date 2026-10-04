using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Enum;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class UpdateAssessmentDto
    {
        public Guid AssessmentId { get; set; }
        public AssessmentDecision Decision { get; set; }

        public decimal ApprovedAmount { get; set; }

    }
}