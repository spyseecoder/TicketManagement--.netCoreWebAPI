using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TktMgmt.Common.DTOs.User
{
    public class UserCreateDto
    {
        [Required]
        [RegularExpression(@"^[A-Za-z]+$",
        ErrorMessage = "First name must contain only alphabets (single word)")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]+( [A-Za-z]+)*$",
        ErrorMessage = "Last name can contain multiple words with alphabets only")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
       ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
