
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitUp.Domain.Models
{
    public class Client : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        public int enteries { get; set; }
        public int allowed_times { get; set; }
        public DateTime day_last_payment { get; set; }
        public bool active { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public char Gender { get; set; }
        [Required]
        public DateTime startDay { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
