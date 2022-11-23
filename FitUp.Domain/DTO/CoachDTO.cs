using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Domain.DTO
{
    public class CoachDTO
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        
        public string Email { get; set; }
      
        public int Age { get; set; }
       
        public char Gender { get; set; }
        
    }
}
