using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Domain.Exceptions
{
    public class AllowedClientEnteries : Exception
    {
        public AllowedClientEnteries() { }

        public AllowedClientEnteries(String s)
            : base(String.Format("Client entery expired for this week: ", s))
        {

        }
    }
}
