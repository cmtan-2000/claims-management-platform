using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Entities;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class ClaimListDto
    {
        public Guid Id { get; set; }
        public DateOnly IncidentDate { get; set; }
        public string IncidentDescription { get; set; } = null!;
        public string ClaimNumber { get; set; } = null!;
        public decimal EstimatedLiability { get; set; }
        public ClaimStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}