using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TktMgmt.Common.DTOs.User
{
    public class UserRoleUpdateDto
    {
        [Required]
        public string UserGuid { get; set; } = string.Empty;
        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}
