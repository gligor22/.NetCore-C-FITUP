
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Domain.Models
{
    public class Reservation : BaseEntity
    {
        public Training Training { get; set; }
        public Guid TrainingID { get; set; }
        public Client Client { get; set; }
        public Guid ClientID { get; set; }

    }
}
