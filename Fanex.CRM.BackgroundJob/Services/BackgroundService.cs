namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly IMessageService messageService;

        public BackgroundService(
            IMaintenanceService maintenanceService,
            ISystemSettingService systemSettingService,
            IMessageService messageService)
        {
            this.maintenanceService = maintenanceService;
            this.systemSettingService = systemSettingService;
            this.messageService = messageService;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [Queue(QueuePriority.Critical)]
        public async Task CreateFeedbackForVoidingAbnormalBet()
        {
            bool isUM = await CheckUM();
            if (!isUM)
            {
                var settingNames = new[]
                {
                    SystemSettingName.JobStatus,
                    SystemSettingName.LastMsgId,
                    SystemSettingName.BatchSite
                };
                IEnumerable<SystemSetting> settings = await systemSettingService.GetSystemSettingAsync(settingNames);
                var isRunning = settings.Any(x => x.SettingName.Equals(SystemSettingName.JobStatus, StringComparison.OrdinalIgnoreCase)
                    && x.Value.Equals(JobStatus.Running, StringComparison.OrdinalIgnoreCase));
                if (!isRunning)
                {
                    var setting = settings.FirstOrDefault(x => x.SettingName.Equals(SystemSettingName.LastMsgId, StringComparison.OrdinalIgnoreCase));
                    long.TryParse(setting?.Value, out long lastMessageId);

                    setting = settings.FirstOrDefault(x => x.SettingName.Equals(SystemSettingName.BatchSite, StringComparison.OrdinalIgnoreCase));
                    int.TryParse(setting?.Value, out int batchSize);

                    await messageService.GetNewMessageAsync(lastMessageId, batchSize);
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