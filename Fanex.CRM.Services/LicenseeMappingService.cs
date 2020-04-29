namespace Fanex.CRM.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Criteria.LicenseeMapping;
    using Fanex.CRM.Models.LicenseeMapping;

    public interface ILicenseeMappingService
    {
        IEnumerable<string> GetLicensee();

        IEnumerable<LicenseeManager> GetLicenseeManager(string username);

        IEnumerable<LicenseeMapping> GetLicenseeMapping(string keyword = null);

        CommandResult UpdateLicenseeMapping(UpdateLicenseeMappingParam param);
    }

    public class LicenseeMappingService : ILicenseeMappingService
    {
        private readonly IBaseService baseService;

        public LicenseeMappingService() : this(null)
        {
        }

        public LicenseeMappingService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public IEnumerable<string> GetLicensee()
        {
            var criteria = new GetLicenseeCriteria();

            return baseService.GetData<string>(criteria);
        }

        public IEnumerable<LicenseeManager> GetLicenseeManager(string username)
        {
            var criteria = new GetLicenseeManagerCriteria
            {
                Username = username
            };
            return baseService.GetData<LicenseeManager>(criteria);
        }

        public IEnumerable<LicenseeMapping> GetLicenseeMapping(string keyword = null)
        {
            var criteria = new GetLicenseeMappingCriteria
            {
                Setting = keyword
            };
            return baseService.GetData<LicenseeMapping>(criteria);
        }

        public CommandResult UpdateLicenseeMapping(UpdateLicenseeMappingParam param)
        {
            var criteria = new UpdateLicenseeMappingCriteria
            {
                SettingId = param.SettingId,
                SourceMapping = param.SourceMapping,
                LicenseeManager = param.LicenseeManager
            };
            return baseService.ExecuteNonQuery(criteria);
        }
    }
}