namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Criteria.Setting;
    using CRM.Models.Setting;

    public interface ISettingService
    {
        Task<IEnumerable<Setting>> GetAsync(GetSettingParam param);
    }

    public class SettingService : ISettingService
    {
        private readonly IBaseService baseService;

        public SettingService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<Setting>> GetAsync(GetSettingParam param)
        {
            var criteria = new GetSettingCriteria
            {
                SettingName = param.SettingName,
                IsActive = param.IsActive
            };
            return await baseService.GetDataAsync<Setting>(criteria);
        }
    }
}