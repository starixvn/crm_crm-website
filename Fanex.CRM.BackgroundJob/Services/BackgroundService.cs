namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Models;
    using Hangfire;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ONEbook.UM.Services;

    public interface IBackgroundService
    {
        Task CreateFeedbackForVoidingAbnormalBet();
    }

    public class BackgroundService : IBackgroundService
    {
        private readonly AppSettings appSettings;
        private readonly IMaintenanceService maintenanceService;
        private readonly IJobRunningChecker jobRunningChecker;
        private readonly ISystemSettingService systemSettingService;
        private readonly IFeedbackService feedbackService;
        private readonly ICRMLogService logService;
        private readonly ILogger mlogger;

        public BackgroundService(
            IOptionsSnapshot<AppSettings> appSettings,
            IMaintenanceService maintenanceService,
            IJobRunningChecker jobRunningChecker,
            ISystemSettingService systemSettingService,
            IFeedbackService feedbackService,
            ICRMLogService logService,
            ILogger<BackgroundService> mlogger)
        {
            this.appSettings = appSettings.Value;
            this.maintenanceService = maintenanceService;
            this.jobRunningChecker = jobRunningChecker;
            this.systemSettingService = systemSettingService;
            this.feedbackService = feedbackService;
            this.logService = logService;
            this.mlogger = mlogger;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [Queue(QueuePriority.Critical)]
        public async Task CreateFeedbackForVoidingAbnormalBet()
        {
            bool isUM = await CheckUM();
            if (isUM)
            {
                mlogger.LogInformation("UM time.");
                return;
            }

            try
            {
                AbnormalBetSystemSetting setting = await systemSettingService.GetAbnormalBetSettingAsync();
                if (!setting.ServiceFlag)
                {
                    mlogger.LogInformation("Service is shutdown.");
                    return;
                }

                var isRunning = setting.JobStatus.Equals(JobStatus.Running, StringComparison.OrdinalIgnoreCase);
                if (!isRunning)
                {
                    await Task.WhenAll(
                        jobRunningChecker.MarkRunning(JobName.CreateFeedbackForVoidingAbnormalBet),
                        feedbackService.CreateFeedbackForVoidingAbnormalBetAsync(setting)
                    );
                    await jobRunningChecker.ClearRunning(JobName.CreateFeedbackForVoidingAbnormalBet);
                }
                else
                {
                    isRunning = await jobRunningChecker.IsRunning(JobName.CreateFeedbackForVoidingAbnormalBet);
                    if (!isRunning && setting.LastRuntime.AddMinutes(appSettings.RetryRuntime) < DateTime.Now)
                    {
                        mlogger.LogInformation("Rerun service.");
                        var settingJobStatus = new List<SystemSettingItem>
                        {
                            new SystemSettingItem { SettingName = SystemSettingName.JobStatus, Value = JobStatus.Completed },
                            new SystemSettingItem { SettingName = SystemSettingName.LastRuntime, Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt") }
                        };
                        await systemSettingService.UpdateSystemSettingAsync(settingJobStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                mlogger.LogInformation(ex.Message);
                await jobRunningChecker.ClearRunning(JobName.CreateFeedbackForVoidingAbnormalBet);
                await logService.ErrorAsync(ex);
            }
        }

        private async Task<bool> CheckUM()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            return maintanceTime.IsUnderMaintenanceTime;
        }
    }
}