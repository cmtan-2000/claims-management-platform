using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Entities;

namespace Claims.Api.Modules.Dashboards.DTOs
{
    public class RecentClaimDto
    {
        public Guid Id { get; set; }

        public string ClaimNumber { get; set; } = null!;

        public ClaimStatus Status { get; set; }

        public DateOnly IncidentDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}