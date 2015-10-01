// --------------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    /// <summary>
    /// Customer Service.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private const string GetCustomerInfoByUsernameSessionKey =
            "Fanex.CRM.Customer.Services.Implements.CustomerService.GetCustomerInfoByUsernameSessionKey";

        private readonly ICustomerRepository _customerRepositoryInstance;

        private readonly ISettingService _settingService;

        public CustomerService()
            : this(null, null)
        {
        }

        public CustomerService(
            ICustomerRepository customerRepositoryInstance,
            ISettingService settingService)
        {
            _customerRepositoryInstance = customerRepositoryInstance ?? new CustomerRepository();
            _settingService = settingService ?? new SettingService();
        }

        public GetCustomerInfoByUsernameCriteria GetCustomerInfoByUsernameCriteria
        {
            get
            {
                return (GetCustomerInfoByUsernameCriteria)SessionExtension.GetSession(
                    HttpContext.Current.Session,
                    GetCustomerInfoByUsernameSessionKey);
            }
        }

        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            var customerInfo = _customerRepositoryInstance.GetCustomerInfoByUsername(criteria);
            if (customerInfo != null)
            {
                customerInfo.Role.Name = RoleSettingService.GetRoleName(customerInfo.Role.Id);
                var siteSetting = _settingService.GetSettingById(new GetSettingByIdCriteria
                {
                    SettingId = customerInfo.SiteSetting.Id
                });
                customerInfo.SiteSetting.Name = siteSetting.Value;
                if (!criteria.IsGuest)
                {
                    customerInfo.UplineInfo = GetUplineInfoOfUser(new GetUplineInfoOfUserCriteria
                    {
                        CustomerId = customerInfo.Id
                    });
                }

                SessionExtension.AddSession(HttpContext.Current.Session, GetCustomerInfoByUsernameSessionKey, criteria);
            }
            else
            {
                SessionExtension.DeleteSession(HttpContext.Current.Session, GetCustomerInfoByUsernameSessionKey);
            }

            return customerInfo;
        }

        public IEnumerable<BaseCustomerInfo> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            var uplineInfoResult = new List<BaseCustomerInfo>();

            var rawData = _customerRepositoryInstance.GetUplineInfoOfUser(criteria);
            foreach (var item in rawData)
            {
                if (criteria.CustomerId == item.Id)
                {
                    continue;
                }

                item.Role.Name = RoleSettingService.GetRoleName(item.Role.Id);
                uplineInfoResult.Add(item);
            }

            return uplineInfoResult;
        }

        public Result UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
        {
            var result = _customerRepositoryInstance.UpdateCustomerInfo(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.CustomerInfo_UpdateSuccess;
            }

            return result;
        }

        public IEnumerable<string> QueryCustomerByUsername(CustomerSuggestionCriteria criteria)
        {
            return _customerRepositoryInstance.GetCustomersByUsername(criteria);
        }

        public bool UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria)
        {
            return _customerRepositoryInstance.UpdateCustomerSuggestion(criteria);
        }

        public IEnumerable<dynamic> RelatedCustomerAutoSuggestion(RelatedCustomerAutoSuggestionCriteria criteria)
        {
            var customers = _customerRepositoryInstance.GetRelatedCustomerAutoSuggestions(criteria);
            return customers.Select(x => new
            {
                key = x.Key,
                value = x.Value
            });
        }

        public IEnumerable<CustomerInfo> VerifyUsername(VerifyRelatedCustomerCriteria criteria)
        {
            return _customerRepositoryInstance.VerifyUsername(criteria);
        }
    }
}
