using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Domain.Exceptions
{
    public class ReservedForTheDay : Exception
    {
        public ReservedForTheDay() { }

        public ReservedForTheDay(String s)
            : base(String.Format("Client already reserved session for selected day: ", s))
        {

        }
    }
}
