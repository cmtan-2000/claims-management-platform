using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class CreateInformationRequestDto
    {
        [Required]
        public string Question { get; set; } = string.Empty;

        [Required]
        [MinLength(10)]
        public string Description { get; set; } = string.Empty;
    }
}