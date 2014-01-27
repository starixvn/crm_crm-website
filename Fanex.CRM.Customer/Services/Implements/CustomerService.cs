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
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Customer.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
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

        public GetCustomerInfoByUsernameCriteria GetCustomerInfoByUsernameCriteria
        {
            get
            {
                return (GetCustomerInfoByUsernameCriteria)SessionExtension.GetSession(
                    HttpContext.Current.Session,
                    GetCustomerInfoByUsernameSessionKey);
            }
        }

        public CustomerService()
            : this(null, null)
        {
        }

        public CustomerService(
            ICustomerRepository repositoryInstance,
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
                    result.SiteSetting.Name = siteSetting.Value;
                    if (!criteria.IsGuest)
                    {
                        result.UplineInfo = GetUplineInfoOfUser(new GetUplineInfoOfUserCriteria
                        {
                            CustomerId = result.Id
                        });
                    }

                    SessionExtension.AddSession(HttpContext.Current.Session, GetCustomerInfoByUsernameSessionKey, criteria);
                }
                else
                {
                    SessionExtension.AddSession(HttpContext.Current.Session, GetCustomerInfoByUsernameSessionKey, null);
                }
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
                if (criteria.CustomerId > 0)
                {
                    updateResult = _repositoryInstance.UpdateCustomerSuggestion(criteria);
                }
                else
                {
                    updateResult = UpdateCustomerSearchCountValue(criteria);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return updateResult;
        }

        public BaseModel UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
        {
            var updateResult = new BaseModel();
            try
            {
                updateResult = _repositoryInstance.UpdateCustomerInfo(criteria);
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

        public bool UpdateCustomerSearchCountValue(UpdateCustomerSuggestionCriteria criteria)
        {
            bool result = false;
            try
            {
                result = _repositoryInstance.UpdateCustomerSearchCountValue(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}
