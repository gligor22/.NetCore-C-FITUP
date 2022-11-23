using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Domain.Exceptions
{
    public class MonthAlreadyPayed : Exception
    {
        public MonthAlreadyPayed() { }

        public MonthAlreadyPayed(String s)
            : base(String.Format("You have already payed for the selected Month. Last payment made on: ", s))
        {

        }
    }
}
