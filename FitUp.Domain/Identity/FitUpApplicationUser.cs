using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitUp.Domain.Identity
{
    public class FitUpApplicationUser : IdentityUser
    {
        [Required]
        public String first_name { get; set; }
        [Required]
        public String last_name { get; set; }
        [Required]
        public String address { get; set; }

    }
}
