namespace Fanex.CRM.Customer.Services
{
    using Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    public class CustomerDetailService : ICustomerDetailService
    {
        private readonly ICustomerDetailRepository _customerRepositoryInstance;
        private readonly ISettingService _settingService;
        private readonly IBaseService baseService;

        public CustomerDetailService() : this(null, null, null)
        {
        }

        public CustomerDetailService(
            ICustomerDetailRepository customerRepositoryInstance,
            IBaseService baseService,
            ISettingService settingService)
        {
            _customerRepositoryInstance = customerRepositoryInstance ?? new CustomerDetailRepository();
            _settingService = settingService ?? new SettingService();
            this.baseService = baseService ?? new BaseService();
        }

        public CustomerInfo GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria)
        {
            var customerInfo = _customerRepositoryInstance.GetCustomerInfoDetailById(criteria);
            if (customerInfo != null)
            {
                customerInfo.Role.Name = RoleSettingService.GetRoleName(customerInfo.Role.Id);
                var siteSetting = _settingService.GetSettingById(new GetSettingByIdCriteria
                {
                    SettingId = customerInfo.SiteSetting.Id
                });
                customerInfo.SiteSetting.Name = siteSetting.Value;
            }

            return customerInfo;
        }

        public CommandResult UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria)
        {
            CommandResult updateResult = baseService.ExecuteNonQuery(criteria);
            if (updateResult.IsSuccessful)
            {
                updateResult.Message = Labels.CustomerInfo_UpdateSuccess;
            }

            return updateResult;
        }

        public dynamic GetAllSettings(UserRole userRole)
        {
            return new
            {
                Titles = _settingService.GetAllTitles(),
                Languages = _settingService.GetAllLanguages(),
                Sites = userRole == UserRole.CN88Staff ? _settingService.GetAllSitesCN88() : _settingService.GetAllSites(),
                LicenseeTypes = _settingService.GetAllLicenseeTypes(),
                Countries = _settingService.GetAllCountries(),
                Provinces = _settingService.GetAllProvinces()
            };
        }
    }
}