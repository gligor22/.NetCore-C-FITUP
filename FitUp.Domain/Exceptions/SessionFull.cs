using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Domain.Exceptions
{
    public class SessionFull : Exception
    {

        public SessionFull()
            : base(String.Format("Session full. Please try other training"))
        {

        }
    }
}
