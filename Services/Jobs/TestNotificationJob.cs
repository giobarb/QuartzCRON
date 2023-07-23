using CronService.Models;
using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CronService.Services.Jobs
{
    class TestNotificationJob : WorkerJob
    {
        public static string JobName = "TestNotificationJob";
        public static string JobGroup = "TestNotificationJobGroup";
        public static string TriggerName = "TestNotificationTrigger";
        public static string TriggerGroup = "TestNotificationTriggerGroup";

        public TestNotificationJob() : base(
            JobName, JobGroup, TriggerName, TriggerGroup,
            AppSettings.TestNotificationTriggerTime, typeof(TestNotificationJob)
            )
        {

        }

        public override IJob GetJob(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<TestNotificationJob>();
        }
        public override Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("Running TestNotificationJob at " + DateTime.Now.ToString());
                var triggerTime = AppSettings.GetValue(AppSettings.TestNotificationTriggerTime);
                //Console.WriteLine($"Current Invoice Notification trigger time: {triggerTime}");
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine("Received an error when executing TestNotificationJob. " + e.Message);
                return Task.FromException(e);
            }
        }
    }
}
