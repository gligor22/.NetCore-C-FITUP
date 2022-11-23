using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Service.Scheduler
{
    public class SingeltonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public SingeltonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle triggerFiredBundle,
        IScheduler scheduler)
        {
            return _serviceProvider.GetService(triggerFiredBundle.JobDetail.JobType) as IJob;
        }
        public void ReturnJob(IJob job) { }
    }
}
