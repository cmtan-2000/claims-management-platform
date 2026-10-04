using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class CreateSettlementDto
    {
        [Required]
        public decimal SettlementAmount { get; set; }

        [Required]
        public string Reference { get; set; } = string.Empty;
    }
}