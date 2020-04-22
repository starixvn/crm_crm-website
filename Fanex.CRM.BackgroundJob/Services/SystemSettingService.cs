namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;

    public interface ISystemSettingService
    {
        Task<AbnormalBetSystemSetting> GetAbnormalBetSettingAsync();

        Task<CommandResult> UpdateSystemSettingAsync(IEnumerable<SystemSettingItem> settings);
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
    }
}