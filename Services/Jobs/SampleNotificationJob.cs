using CronService.Models;
using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CronService.Services.Jobs
{
    /// <summary>
    /// This class simulates generating Sample notification. This is a job which is managd by the JobFactoryFloor
    /// </summary>
    public class SampleNotificationJob : WorkerJob
    {
        public static string JobName = "SampleNotificationJob";
        public static string JobGroup = "SampleNotificationJobGroup";
        public static string TriggerName = "SampleNotificationTrigger";
        public static string TriggerGroup = "SampleNotificationTriggerGroup";

        public SampleNotificationJob() : base(
            JobName, JobGroup, TriggerName, TriggerGroup, 
            AppSettings.SampleNotificationTriggerTime, typeof(SampleNotificationJob)
            )
        {

        }

        public override IJob GetJob(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<SampleNotificationJob>();
        }
        public override Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("Running SampleNotificationJob at " + DateTime.Now.ToString());
                var triggerTime = AppSettings.GetValue(AppSettings.SampleNotificationTriggerTime);
                //Console.WriteLine($"Current SampleNotification trigger time: {triggerTime}");
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine("Received an error when executing SampleNotificationJob. " + e.Message);
                return Task.FromException(e);
            }
        }
    }
}
