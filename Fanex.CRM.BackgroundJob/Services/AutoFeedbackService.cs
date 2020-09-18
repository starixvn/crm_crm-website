namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Models;
    using global::CRM.Core.Services;
    using Microsoft.Extensions.Options;

    public interface IAutoFeedbackService
    {
        Task ExecuteJobAsync(AbnormalBetSystemSetting setting);
    }

    public class AutoFeedbackService : IAutoFeedbackService
    {
        private readonly AppJobSettings appSettings;
        private readonly IJobRunningChecker jobRunningChecker;
        private readonly ISystemSettingService systemSettingService;
        private readonly IFeedbackService feedbackService;
        private readonly ICRMLogService logService;

        public AutoFeedbackService(
            IOptionsSnapshot<AppJobSettings> appSettings,
            IJobRunningChecker jobRunningChecker,
            ISystemSettingService systemSettingService,
            IFeedbackService feedbackService,
            ICRMLogService logService)
        {
            this.appSettings = appSettings.Value;
            this.jobRunningChecker = jobRunningChecker;
            this.systemSettingService = systemSettingService;
            this.feedbackService = feedbackService;
            this.logService = logService;
        }

        public async Task ExecuteJobAsync(AbnormalBetSystemSetting setting)
        {
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
                    logService.LogInformation("Rerun service.");
                    var settingJobStatus = new List<SystemSettingItem>
                        {
                            new SystemSettingItem { SettingName = SystemSettingName.JobStatus, Value = JobStatus.Completed },
                            new SystemSettingItem { SettingName = SystemSettingName.LastRuntime, Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt") }
                        };
                    await systemSettingService.EditSystemSettingAsync(settingJobStatus);
                }
            }
        }
    }
}