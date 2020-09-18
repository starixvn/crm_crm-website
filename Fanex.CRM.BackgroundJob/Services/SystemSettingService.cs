namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Criteria.ChatBot;
    using Fanex.CRM.BackgroundJob.Models;
    using global::CRM.Core.Extensions.XML;
    using global::CRM.Core.Models;
    using global::CRM.Core.Services;

    public interface ISystemSettingService
    {
        Task<AbnormalBetSystemSetting> GetAbnormalBetSettingAsync();

        Task<CommandResult> EditSystemSettingAsync(IEnumerable<SystemSettingItem> settings);

        Task<CommandResult> EditJobStatusAsync(string status);

        Task<AbnormalBetSystemSetting> GetChatBotSystemSettingAsync();
    }

    public class SystemSettingService : ISystemSettingService
    {
        private readonly IBaseService baseService;

        public SystemSettingService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<AbnormalBetSystemSetting> GetAbnormalBetSettingAsync()
        {
            var criteria = new GetAbnormalBetSystemSettingCriteria();
            return await baseService.GetDataEntityAsync<AbnormalBetSystemSetting>(criteria);
        }

        public async Task<CommandResult> EditSystemSettingAsync(IEnumerable<SystemSettingItem> settings)
        {
            var criteria = new EditSystemSettingCommand
            {
                Settings = settings.ToXML("ROOT").ToString()
            };
            return await baseService.ExecuteNonQueryAsync(criteria);
        }

        public async Task<CommandResult> EditJobStatusAsync(string status)
        {
            var settings = new List<SystemSettingItem>
            {
                new SystemSettingItem { SettingName = SystemSettingName.JobStatus, Value = status }
            };
            return await EditSystemSettingAsync(settings);
        }

        public async Task<AbnormalBetSystemSetting> GetChatBotSystemSettingAsync()
        {
            var criteria = new GetChatBotSystemSettingCriteria();
            return await baseService.GetDataEntityAsync<AbnormalBetSystemSetting>(criteria);
        }
    }
}