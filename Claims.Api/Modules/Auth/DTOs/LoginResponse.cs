using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Claims.Api.Modules.Auth.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

    }
}