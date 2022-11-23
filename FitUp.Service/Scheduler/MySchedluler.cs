using FitUp.Service.Jobs;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl.AdoJobStore.Common;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FitUp.Service.Scheduler
{
    public class MySchedluler : IHostedService
    {

        public IScheduler scheduler { get; set; }
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<MyJob> _jobs;

        public MySchedluler(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, IEnumerable<MyJob> jobs)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobs = jobs;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            scheduler = await _schedulerFactory.GetScheduler();
            scheduler.JobFactory = _jobFactory;

            foreach (var job in _jobs)
            {
                var j = CreateJob(job);
                var trigger = CreateTrigger(job);

                await scheduler.ScheduleJob(j, trigger, cancellationToken);
                await scheduler.Start(cancellationToken);
            }

        }

        public static ITrigger CreateTrigger(MyJob myJob)
        {
            return TriggerBuilder.Create().WithIdentity($"{myJob.JobType.FullName}.trigger").WithCronSchedule(myJob.Expression).WithDescription(myJob.Expression).Build();
        }

        public static IJobDetail CreateJob(MyJob myJob)
        {
            var type = myJob.JobType;
            return JobBuilder.Create(type).WithIdentity(type.FullName).WithDescription(type.Name).Build();
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await scheduler.Shutdown();
        }
    }
}
