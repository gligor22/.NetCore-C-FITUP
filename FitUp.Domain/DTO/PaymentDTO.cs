using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Domain.DTO
{
    public class PaymentDTO
    {
        public int Payed_Price { get; set; }
        public DateTime payment_date { get; set; }
        public Guid ClientID { get; set; }
        
    }
}
