
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitUp.Domain.Models
{
    public class Training : BaseEntity
    {
        [Required]
        [EnumDataType(typeof(TrainingType))]
        public TrainingType type { get; set; }
        [Required]
        public int  duration { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        public int capacity { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public Coach Coach { get; set; }
        public Guid CoachID { get; set; }

        public override string ToString()
        {
            return type + " " + dateTime;
        }

    }
}
