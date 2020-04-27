namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Models;
    using Hangfire;
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

        public BackgroundService(
            IMaintenanceService maintenanceService,
            ISystemSettingService systemSettingService,
            IFeedbackService feedbackService)
        {
            this.maintenanceService = maintenanceService;
            this.systemSettingService = systemSettingService;
            this.feedbackService = feedbackService;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [Queue(QueuePriority.Critical)]
        public async Task CreateFeedbackForVoidingAbnormalBet()
        {
            bool isUM = await CheckUM();
            if (!isUM)
            {
                AbnormalBetSystemSetting setting = await systemSettingService.GetAbnormalBetSettingAsync();
                var isRunning = setting.JobStatus.Equals(JobStatus.Running, StringComparison.OrdinalIgnoreCase);
                if (!isRunning)
                {
                    await feedbackService.CreateFeedbackForVoidingAbnormalBetAsync(setting);
                }
            }
        }

        private async Task<bool> CheckUM()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            return maintanceTime.IsUnderMaintenanceTime;
        }
    }
}