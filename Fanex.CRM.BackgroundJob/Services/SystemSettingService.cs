namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;

    public interface ISystemSettingService
    {
        Task<IEnumerable<SystemSetting>> GetSystemSettingAsync(string[] settingNames);
    }

    public class SystemSettingService : ISystemSettingService
    {
        private readonly IBaseService baseService;

        public SystemSettingService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<SystemSetting>> GetSystemSettingAsync(string[] settingNames)
        {
            var criteria = new GetSystemSettingByNameCriteria { SettingNames = string.Join(",", settingNames) };
            return await baseService.GetDataAsync<SystemSetting>(criteria);
        }
    }
}