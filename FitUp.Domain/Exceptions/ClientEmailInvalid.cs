using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Domain.Exceptions
{
    public class ClientEmailInvalid : Exception
    {
        public ClientEmailInvalid() { }

        public ClientEmailInvalid(String s)
            : base(String.Format("Client Email Invalid. No Client found with this email. ", s))
        {

        }
    }
}
