﻿using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Models
{
    public class LoginModel
    {
        [Key]
        public int LoginId { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        
        public string Password { get; set; }
    }
}
