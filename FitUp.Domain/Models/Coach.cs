
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitUp.Domain.Models
{
    public class Coach : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public char Gender { get; set; }
        [Required]
        public DateTime entryDay { get; set; }
        public float Rate { get; set; }
        public int num_raters { get; set; }
        public int Trainings_Weekly { get; set; }
        public int Trainings_Monthly { get; set; }
        public int Trainings_Yearly { get; set; }
        public int All_training_num { get; set; }
        public ICollection<Training> Trainings { get; set; }

        public override string ToString()
        {
            return FirstName+" "+LastName;
        }

    }
}
