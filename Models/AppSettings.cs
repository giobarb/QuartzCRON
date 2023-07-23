using Microsoft.Extensions.Configuration;

namespace CronService.Models
{
    public static class AppSettings
    {
        public static string JobFactoryTriggerTime = "QuartzSchedule:JobFactoryFloorTriggerTime";
        public static string SampleNotificationTriggerTime = "QuartzSchedule:SampleNotificationTriggerTime";
        public static string TestNotificationTriggerTime = "QuartzSchedule:TestNotificationTriggerTime";

        public static string GetValue(string key)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            return configuration.GetSection(key).Value;
        }
    }
}
