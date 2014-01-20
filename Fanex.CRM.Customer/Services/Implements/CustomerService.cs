// --------------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories.Implements;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Customer.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Customer Service.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly string GetCustomerInfoByUsernameSessionKey = "Fanex.CRM.Customer.Services.Implements.CustomerService.GetCustomerInfoByUsernameSessionKey";

        private readonly ICustomerRepository _repositoryInstance;

        private readonly ISettingService _settingService;

        public CustomerService()
            : this(null, null)
        {
        }

        public CustomerService(ICustomerRepository repositoryInstance,
            ISettingService settingService)
        {
            _repositoryInstance = repositoryInstance ?? new CustomerRepository();
            _settingService = settingService ?? new SettingService();
        }

        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            CustomerInfo result = null;

            try
            {
                var rawData = _repositoryInstance.GetCustomerInfoByUsername(criteria);
                result = rawData;
                if (result != null)
                {
                    result.Role.Name = RoleSettingService.GetRoleName(result.Role.Id);
                    var siteSetting = _settingService.GetSettingById(new GetSettingByIdCriteria
                    {
                        SettingId = result.SiteSetting.Id
                    });
                    result.SiteSetting.Name = siteSetting.Name;
                    result.UplineInfo = GetUplineInfoOfUser(new GetUplineInfoOfUserCriteria
                    {
                        CustomerId = result.Id
                    });
                }

                //SessionExtension.AddSession(HttpContext.Current.Session, GetCustomerInfoByUsernameSessionKey, criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<CustomerInfo> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            List<CustomerInfo> result = new List<CustomerInfo>();
            try
            {
                var rawData = _repositoryInstance.GetUplineInfoOfUser(criteria);
                for (var i = 0; i < rawData.Count; i++)
                {
                    var uplineInfo = rawData[i];
                    if (criteria.CustomerId == uplineInfo.Id)
                    {
                        continue;
                    }

                    uplineInfo.Role.Name = RoleSettingService.GetRoleName(uplineInfo.Role.Id);
                    result.Add(uplineInfo);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public bool UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria)
        {
            var updateResult = false;
            try
            {
                updateResult = _repositoryInstance.UpdateCustomerSuggestion(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return updateResult;
        }
    }
}
