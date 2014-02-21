// --------------------------------------------------------------------------
// <copyright file="CustomerDetailService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services.Implements
{
    using System;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories.Implements;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Customer.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

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

        /// <summary>
        /// Gets the customer information detail by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer information detail by identifier.</returns>
        public CustomerInfo GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria)
        {
            CustomerInfo customerInfo = null;
            try
            {
                var rawData = _customerRepositoryInstance.GetCustomerInfoDetailById(criteria);
                customerInfo = rawData;
                if (customerInfo != null)
                {
                    customerInfo.Role.Name = RoleSettingService.GetRoleName(customerInfo.Role.Id);
                    var siteSetting = _settingService.GetSettingById(new GetSettingByIdCriteria
                    {
                        SettingId = customerInfo.SiteSetting.Id
                    });
                    customerInfo.SiteSetting.Name = siteSetting.Value;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return customerInfo;
        }

        /// <summary>
        /// Updates the customer information detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public BaseModel UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria)
        {
            var updateResult = new BaseModel();
            try
            {
                updateResult = _customerRepositoryInstance.UpdateCustomerInfoDetail(criteria);
                if (updateResult != null &&
                    updateResult.Id == ErrorCodeConstants.NoneError)
                {
                    updateResult.Name = Labels.CustomerInfo_UpdateSuccess;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return updateResult;
        }
    }
}
