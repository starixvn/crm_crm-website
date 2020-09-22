namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Models;
    using global::CRM.Core.Services;
    using global::CRM.Letstalk.Services.Services.Interfaces;
    using Hangfire;
    using ONEbook.UM.Services;

    public interface IBackgroundService
    {
        Task CreateFeedbackForVoidingAbnormalBet();

        Task PullMessageForChatBotAsync();

        Task ScanAndSenLetstalkNotification();

        Task MonitorLetstalkService();
    }

    public class BackgroundService : IBackgroundService
    {
        private readonly IMaintenanceService maintenanceService;
        private readonly IJobRunningChecker jobRunningChecker;
        private readonly ISystemSettingService systemSettingService;
        private readonly IAutoFeedbackService feedbackService;
        private readonly IChatBotJobService chatBotJobService;
        private readonly ICRMLogService logService;
        private readonly ISendNotificationService sendNotificationService;
        private readonly IMonitorService monitorService;

        public BackgroundService(
            IMaintenanceService maintenanceService,
            IJobRunningChecker jobRunningChecker,
            ISystemSettingService systemSettingService,
            IAutoFeedbackService feedbackService,
            IChatBotJobService chatBotJobService,
            ICRMLogService logService,
            ISendNotificationService sendNotificationService,
            IMonitorService monitorService)
        {
            this.maintenanceService = maintenanceService;
            this.jobRunningChecker = jobRunningChecker;
            this.systemSettingService = systemSettingService;
            this.feedbackService = feedbackService;
            this.chatBotJobService = chatBotJobService;
            this.logService = logService;
            this.sendNotificationService = sendNotificationService;
            this.monitorService = monitorService;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [Queue(QueuePriority.Critical)]
        public async Task CreateFeedbackForVoidingAbnormalBet()
        {
            try
            {
                bool isUM = await CheckUM();
                if (isUM)
                {
                    logService.LogInformation("UM time.");
                    return;
                }

                AbnormalBetSystemSetting setting = await systemSettingService.GetAbnormalBetSettingAsync();
                if (!setting.ServiceFlag)
                {
                    logService.LogInformation("Service is shutdown.");
                    return;
                }

                await feedbackService.ExecuteJobAsync(setting);
            }
            catch (Exception ex)
            {
                await jobRunningChecker.ClearRunning(JobName.CreateFeedbackForVoidingAbnormalBet);
                await logService.ErrorAsync(ex);
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [Queue(QueuePriority.Critical)]
        public async Task PullMessageForChatBotAsync()
        {
            try
            {
                bool isUM = await CheckUM();
                if (isUM)
                {
                    logService.LogInformation("UM time.");
                    return;
                }

                AbnormalBetSystemSetting setting = await systemSettingService.GetChatBotSystemSettingAsync();
                if (!setting.ServiceFlag)
                {
                    logService.LogInformation("Chat Bot Service is shutdown.");
                    return;
                }

                await chatBotJobService.PullMessageAsync(setting);
                await sendNotificationService.SendNotificationToLetstalk();
            }
            catch (Exception ex)
            {
                await logService.ErrorAsync(ex);
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [Queue(QueuePriority.Critical)]
        public async Task ScanAndSenLetstalkNotification()
        {
            try
            {
                bool isUM = await CheckUM();
                if (isUM)
                {
                    logService.LogInformation("UM time.");
                    return;
                }

                await sendNotificationService.SendNotificationToLetstalk();
            }
            catch (Exception ex)
            {
                await logService.ErrorAsync(ex);
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [Queue(QueuePriority.Default)]
        public async Task MonitorLetstalkService()
        {
            await monitorService.MonitorLetstalkService();
        }

        private async Task<bool> CheckUM()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            return maintanceTime.IsUnderMaintenanceTime;
        }
    }
}