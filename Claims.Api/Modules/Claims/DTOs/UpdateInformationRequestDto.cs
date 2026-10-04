using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Claims.DTOs
{
    public class UpdateInformationRequestDto
    {
        [Required]
        public string Response { get; set; } = string.Empty;
    }
}