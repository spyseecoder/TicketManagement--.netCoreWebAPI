using System;
using System.Collections.Generic;
using System.Text;

namespace TktMgmt.Common.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
