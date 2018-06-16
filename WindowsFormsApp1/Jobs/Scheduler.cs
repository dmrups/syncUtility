using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Jobs
{
    internal static class Scheduler
    {
        public static async Task StartAsync()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();

            IScheduler sched = await factory.GetScheduler();
            await sched.Start();

            IJobDetail job = JobBuilder.Create<SyncJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                //.WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(24))
              .Build();

            await sched.ScheduleJob(job, trigger);
        }

    }
}
