using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitUp.Service.Interfaces
{
    public interface IBackgroundEmailSender
    {
        Task doWork();
    }
}
