namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Models;
    using Hangfire;
    using Hangfire.Storage;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.WindowsServices;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    [DesignerCategory("Code")]
    public class CustomWebHostService : WebHostService
    {
        private readonly IEnumerable<HangfireJobSetting> hangfireJobSettings;
        private readonly IRecurringJobManager recurringJob;

        public CustomWebHostService(IWebHost host) : base(host)
        {
            hangfireJobSettings = host.Services.GetService<IOptionsMonitor<IEnumerable<HangfireJobSetting>>>().CurrentValue;
            recurringJob = host.Services.GetService<IRecurringJobManager>();
        }

        protected override void OnStarted()
        {
            RunHangfireJobs();
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            RemoveCurrentHangfireServer();
            base.OnStopping();
        }

        public void RunHangfireJobs()
        {
            if (IsEnabledJob(HangfireJobName.CreateFeedbackForVoidingAbnormalBet))
            {
                recurringJob.AddOrUpdate<IBackgroundService>("Create Feedback For Voiding Abnormal Bet",
                   job => job.CreateFeedbackForVoidingAbnormalBet(),
                   Cron.Minutely,
                   TimeZoneInfo.Local, QueuePriority.Critical);
            }
        }

        private static void RemoveCurrentHangfireServer()
        {
            IMonitoringApi monitoringApi = JobStorage.Current.GetMonitoringApi();
            var serverToRemove = monitoringApi.Servers()
                .FirstOrDefault(s => s.Name.ToLowerInvariant().Contains(Environment.MachineName.ToLowerInvariant()));
            if (serverToRemove != null)
            {
                JobStorage.Current.GetConnection().RemoveServer(serverToRemove?.Name);
            }
        }

        public bool IsEnabledJob(string jobName)
        {
            var settings = hangfireJobSettings.FirstOrDefault(job => job.JobName.Equals(jobName, StringComparison.OrdinalIgnoreCase));

            return settings != null && settings.IsEnabled;
        }
    }
}