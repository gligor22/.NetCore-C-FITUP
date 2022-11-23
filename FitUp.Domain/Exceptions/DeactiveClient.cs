using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Domain.Exceptions
{
    public class DeactiveClient : Exception
    {
        public DeactiveClient() { }

        public DeactiveClient(String s)
            : base(String.Format("Deactive Client:", s))
        {

        }
    }
}
