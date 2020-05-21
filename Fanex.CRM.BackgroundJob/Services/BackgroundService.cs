namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
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
        private readonly ISystemSettingService systemSettingService;
        private readonly IFeedbackService feedbackService;
        private readonly ILogger mlogger;
        private readonly Logging.ILogger logger;

        public BackgroundService(
            IOptionsSnapshot<AppSettings> appSettings,
            IMaintenanceService maintenanceService,
            ISystemSettingService systemSettingService,
            IFeedbackService feedbackService,
            ILogger<BackgroundService> mlogger,
            Logging.ILogger logger)
        {
            this.appSettings = appSettings.Value;
            this.maintenanceService = maintenanceService;
            this.systemSettingService = systemSettingService;
            this.feedbackService = feedbackService;
            this.mlogger = mlogger;
            this.logger = logger;
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
                    await feedbackService.CreateFeedbackForVoidingAbnormalBetAsync(setting);
                }
                else
                {
                    if(setting.LastRuntime.AddMinutes(appSettings.RetryRuntime) < DateTime.Now)
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
                await WriteLogAsync(ex);
            }
        }

        private async Task<bool> CheckUM()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            return maintanceTime.IsUnderMaintenanceTime;
        }

        private async Task WriteLogAsync(Exception exception)
        {
            var logMessage = new LogMessage(exception.Message)
                       .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                       .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                       .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                       .ToString();
            await logger.ErrorAsync(logMessage);
        }
    }
}