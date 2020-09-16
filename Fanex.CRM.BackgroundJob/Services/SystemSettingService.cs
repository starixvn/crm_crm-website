namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Criteria.ChatBot;
    using Fanex.CRM.BackgroundJob.Models;
    using global::CRM.Core.Models;
    using global::CRM.Core.Services;

    public interface ISystemSettingService
    {
        Task<AbnormalBetSystemSetting> GetAbnormalBetSettingAsync();

        Task<CommandResult> UpdateSystemSettingAsync(IEnumerable<SystemSettingItem> settings);

        Task<CommandResult> UpdateJobStatusAsync(string status);

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

        public async Task<CommandResult> UpdateSystemSettingAsync(IEnumerable<SystemSettingItem> settings)
        {
            var xml = new StringBuilder("<ROOT>");
            foreach (SystemSettingItem setting in settings)
            {
                xml.AppendFormat("<r Name='{0}' Value='{1}' />", setting.SettingName, setting.Value);
            }
            xml.Append("</ROOT>");

            var criteria = new UpdateSystemSettingCriteria
            {
                Settings = xml.ToString()
            };
            return await baseService.ExecuteNonQueryAsync(criteria);
        }

        public async Task<CommandResult> UpdateJobStatusAsync(string status)
        {
            var settings = new List<SystemSettingItem>
            {
                new SystemSettingItem { SettingName = SystemSettingName.JobStatus, Value = status }
            };
            return await UpdateSystemSettingAsync(settings);
        }

        public async Task<AbnormalBetSystemSetting> GetChatBotSystemSettingAsync()
        {
            var criteria = new GetChatBotSystemSettingCriteria();
            return await baseService.GetDataEntityAsync<AbnormalBetSystemSetting>(criteria);
        }
    }
}