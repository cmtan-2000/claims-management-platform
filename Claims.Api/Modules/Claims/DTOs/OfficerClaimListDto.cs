using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Entities;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class OfficerClaimListDto
    {
        public Guid Id { get; set; }
        public string ClaimNumber { get; set; } = null!;
        public Guid ClaimantId { get; set; }
        public Guid? AssignedOfficerId { get; set; }
        public ClaimStatus Status { get; set; }
        public decimal EstimatedLiability { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}