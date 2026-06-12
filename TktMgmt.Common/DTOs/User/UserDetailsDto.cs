using System;
using System.Collections.Generic;
using System.Text;

namespace TktMgmt.Common.DTOs.User
{
    public class UserDetailsDto
    {
        public string UserGuid { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}
