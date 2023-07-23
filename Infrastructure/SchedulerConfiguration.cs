using CronService.Models;
using CronService.Services.JobFactory;
using CronService.Services.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace CronService.Infrastructure
{
    public static class SchedulerConfiguration
    {

        

        static async Task<IScheduler> ConfigureWorkers(IScheduler scheduler)
        {
            foreach(var worker in JobFactoryFloor.Workers)
            {
                scheduler = await worker.ConfigureScheduler(scheduler);
            }
            return scheduler;
        }

        static async Task<IScheduler> ConfigureJobFactoryFloor(IScheduler jobFactoryScheduler)
        {
            // Configure Scheduler for JobFactoryFloor
            var JobFactoryFlootTriggerTime = AppSettings.GetValue(AppSettings.JobFactoryTriggerTime);
            Console.WriteLine($"Configuring JobFactoryFloor Scheduler with Trigger time: {JobFactoryFlootTriggerTime}");

            var JobFactoryJob = JobBuilder.Create<JobFactory>()
                .WithIdentity(JobFactory.JobName, JobFactory.JobGroup)
                .Build();

            

            var JobFactoryFloorTrigger = TriggerBuilder.Create()
                .WithIdentity(JobFactory.TriggerName, JobFactory.TriggerGroup)
                .WithCronSchedule(JobFactoryFlootTriggerTime)
                .Build();

            await jobFactoryScheduler.ScheduleJob(JobFactoryJob, JobFactoryFloorTrigger);            
            
            return jobFactoryScheduler;
        }

        public static async Task Configure(IServiceCollection services)
        {
            try
            {
                var serviceProvider = services.BuildServiceProvider();

                var factory = new StdSchedulerFactory();
                var scheduler = await factory.GetScheduler();
                scheduler.JobFactory = new JobFactoryFloor(serviceProvider, scheduler);

                await ConfigureWorkers(scheduler);
                await ConfigureJobFactoryFloor(scheduler);

                // Start Scheduler
                Console.WriteLine("Starting Scheduler");
                await scheduler.Start();

                await Task.Delay(TimeSpan.FromSeconds(1));
                
            } catch(Exception e)
            {
                Console.WriteLine("Error configuring scheduler. " + e.Message);
            }
        }
    }
}