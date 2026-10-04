using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class CreateClaimDto
    {
        public string ClaimNumber { get; set; } = null!;
        public Guid ClaimantId { get; set; }

        public Guid PolicyId { get; set; }

        public DateOnly IncidentDate { get; set; }

        public string IncidentDescription { get; set; } = null!;

        public decimal EstimatedLiability { get; set; }

    }
}