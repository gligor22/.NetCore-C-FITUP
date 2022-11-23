using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Service.Jobs
{
    public class MyJob
    {
        public Type JobType { get; set; }
        public string Expression { get; set; }

        public MyJob(Type jobType, string expression)
        {
            JobType = jobType;
            Expression = expression;
        }
    }
}
