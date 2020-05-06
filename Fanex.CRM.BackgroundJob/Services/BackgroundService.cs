namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Hangfire;
    using Microsoft.Extensions.Logging;
    using ONEbook.UM.Services;

    public interface IBackgroundService
    {
        Task CreateFeedbackForVoidingAbnormalBet();
    }

    public class BackgroundService : IBackgroundService
    {
        private readonly IMaintenanceService maintenanceService;
        private readonly ISystemSettingService systemSettingService;
        private readonly IFeedbackService feedbackService;
        private readonly Logging.ILogger logger;
        private readonly ILogger mlogger;

        public BackgroundService(
            IMaintenanceService maintenanceService,
            ISystemSettingService systemSettingService,
            IFeedbackService feedbackService,
            Logging.ILogger logger,
            ILogger mlogger)
        {
            this.maintenanceService = maintenanceService;
            this.systemSettingService = systemSettingService;
            this.feedbackService = feedbackService;
            this.logger = logger;
            this.mlogger = mlogger;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [Queue(QueuePriority.Critical)]
        public async Task CreateFeedbackForVoidingAbnormalBet()
        {
            bool isUM = await CheckUM();
            if (!isUM)
            {
                try
                {
                    AbnormalBetSystemSetting setting = await systemSettingService.GetAbnormalBetSettingAsync();
                    var isRunning = setting.JobStatus.Equals(JobStatus.Running, StringComparison.OrdinalIgnoreCase);
                    if (!isRunning)
                    {
                        await feedbackService.CreateFeedbackForVoidingAbnormalBetAsync(setting);
                    }
                }
                catch (Exception ex)
                {
                    mlogger.LogError(ex.Message);
                    await WriteLogAsync(ex);
                }
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