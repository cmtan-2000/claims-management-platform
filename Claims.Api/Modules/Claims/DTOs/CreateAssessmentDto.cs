using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Enum;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class CreateAssessmentDto
    {
        public string AssessmentNotes { get; set; } = null!;

        public decimal EstimatedLoss { get; set; }

    }
}