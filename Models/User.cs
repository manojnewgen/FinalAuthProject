using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FinalAuthProject.Models
{
    public class User: IdentityUser
    {
         [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Role { get; set; } = "User";
    }
}