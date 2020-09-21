namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using global::CRM.Core.Services;

    public interface ISettingService
    {
        Task<IEnumerable<SettingItem>> GetAllSettingAsync();
    }

    public class SettingService : ISettingService
    {
        private readonly IBaseService baseService;

        public SettingService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<SettingItem>> GetAllSettingAsync()
        {
            var criteria = new GetAllSettingCriteria();
            var settings = await baseService.GetDataAsync<SettingItem>(criteria);
            if (settings.Any())
            {
                settings = settings.Where(x => x.IsActive);
                IEnumerable<SettingItem> relations = await GetAllSettingRelationAsync();
                foreach (SettingItem setting in settings)
                {
                    setting.ParentIds = relations.Where(x => x.SettingId == setting.SettingId).Select(x => x.ParentId);
                }
            }

            return settings;
        }

        private async Task<IEnumerable<SettingItem>> GetAllSettingRelationAsync()
        {
            var criteria = new GetAllSettingRelationCriteria();
            return await baseService.GetDataAsync<SettingItem>(criteria);
        }
    }
}