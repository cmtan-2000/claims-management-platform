using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Claims.Api.Modules.Claims.Enum;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class CreatePolicyDto
    {

        public string PolicyNumber { get; set; } = string.Empty;

        public PolicyType PolicyType { get; set; }

        public Market Market { get; set; }
    }
}