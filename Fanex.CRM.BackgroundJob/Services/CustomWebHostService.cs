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
        private readonly IJobRunningChecker jobRunningChecker;

        public CustomWebHostService(IWebHost host) : base(host)
        {
            hangfireJobSettings = host.Services.GetService<IOptions<List<HangfireJobSetting>>>().Value;
            recurringJob = host.Services.GetService<IRecurringJobManager>();
            jobRunningChecker = host.Services.GetService<IJobRunningChecker>();
        }

        protected override void OnStarted()
        {
            jobRunningChecker.ClearRunning(JobName.CreateFeedbackForVoidingAbnormalBet);
            RunHangfireJobs();
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            RemoveCurrentHangfireServer();
            base.OnStopping();
        }

        protected internal virtual void RunHangfireJobs()
        {
            if (IsEnabledJob(HangfireJobName.CreateFeedbackForVoidingAbnormalBet))
            {
                recurringJob.AddOrUpdate<IBackgroundService>("Create Feedback For Voiding Abnormal Bet",
                   job => job.CreateFeedbackForVoidingAbnormalBet(),
                   Cron.Minutely,
                   TimeZoneInfo.Local, QueuePriority.Critical);
            }

            if (IsEnabledJob(HangfireJobName.PullMessageForChatBot))
            {
                var cronExpression = GetCronExpression(HangfireJobName.PullMessageForChatBot);
                recurringJob.AddOrUpdate<IBackgroundService>("Pull Message For Chat Bot",
                   job => job.PullMessageForChatBotAsync(),
                   cronExpression,
                   TimeZoneInfo.Local, QueuePriority.Critical);
            }

            if (IsEnabledJob(HangfireJobName.ScanAndSendLetstalkNotification))
            {
                recurringJob.AddOrUpdate<IBackgroundService>("Scan and send Letstalk Notification",
                  job => job.ScanAndSenLetstalkNotification(),
                  Cron.Minutely,
                  TimeZoneInfo.Local, QueuePriority.Critical);
            }

            if (IsEnabledJob(HangfireJobName.MonitorLetstalkService))
            {
                var cronExpression = GetCronExpression(HangfireJobName.MonitorLetstalkService);
                recurringJob.AddOrUpdate<IBackgroundService>("Monitor Letstalk Service",
                  job => job.MonitorLetstalkService(),
                  cronExpression,
                  TimeZoneInfo.Local);
            }
        }

        protected internal virtual void RemoveCurrentHangfireServer()
        {
            IMonitoringApi monitoringApi = JobStorage.Current.GetMonitoringApi();
            var serverToRemove = monitoringApi.Servers()
                .FirstOrDefault(s => s.Name.ToLowerInvariant().Contains(Environment.MachineName.ToLowerInvariant()));
            if (serverToRemove != null)
            {
                JobStorage.Current.GetConnection().RemoveServer(serverToRemove?.Name);
            }
        }

        protected internal virtual bool IsEnabledJob(string jobName)
        {
            var settings = hangfireJobSettings.FirstOrDefault(job => job.JobName.Equals(jobName, StringComparison.OrdinalIgnoreCase));

            return settings != null && settings.IsEnabled;
        }

        protected internal virtual string GetCronExpression(string jobName)
        {
            var settings = hangfireJobSettings.FirstOrDefault(job => job.JobName.Equals(jobName, StringComparison.OrdinalIgnoreCase));
            return settings != null ? settings.CronExpression : string.Empty;
        }
    }
}