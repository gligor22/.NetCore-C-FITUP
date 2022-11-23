
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitUp.Domain.Models
{
    public class Payment : BaseEntity
    {
        [Required]
        public int Payed_Price { get; set; }
        [Required]
        public int week_number_trainings { get; set; }
        [Required]
        public DateTime payment_date { get; set; }
        [Required]
        public Client Client { get; set; }
        [Required]
        public Guid ClientID { get; set; }

        public override string ToString()
        {
            return Client.FirstName + " " + Client.LastName + " - " + payment_date.ToString().Split(' ')[0] +
                " - " + week_number_trainings.ToString() + " - " + Payed_Price +"den";
        }
    }
}
