using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Entities;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class UpdateClaimStatusDto
    {
        public ClaimStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}