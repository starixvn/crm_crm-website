// --------------------------------------------------------------------------
// <copyright file="CustomerDetailService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services
{
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    /// <summary>
    /// Customer Detail Service.
    /// </summary>
    public class CustomerDetailService : ICustomerDetailService
    {
        private readonly ICustomerDetailRepository _customerRepositoryInstance;

        private readonly ISettingService _settingService;

        public CustomerDetailService()
            : this(null, null)
        {
        }

        public CustomerDetailService(
            ICustomerDetailRepository customerRepositoryInstance,
            ISettingService settingService)
        {
            _customerRepositoryInstance = customerRepositoryInstance ?? new CustomerDetailRepository();
            _settingService = settingService ?? new SettingService();
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

        public Result UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria)
        {
            var updateResult = _customerRepositoryInstance.UpdateCustomerInfoDetail(criteria);
            if (updateResult.Code == ErrorCodeConstants.NoneError)
            {
                updateResult.Message = Labels.CustomerInfo_UpdateSuccess;
            }

            return updateResult;
        }

        public dynamic GetAllSettings()
        {
            return new
            {
                Titles = _settingService.GetAllTitles(),
                Languages = _settingService.GetAllLanguages(),
                Sites = _settingService.GetAllSites(),
                LicenseeTypes = _settingService.GetAllLicenseeTypes(),
                Countries = _settingService.GetAllCountries()
            };
        }
    }
}
