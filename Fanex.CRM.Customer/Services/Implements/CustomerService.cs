// --------------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;
    using Setting.Constants;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Customer Service.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly ISettingService _settingService;

        public CustomerService()
            : this(null, null)
        {
        }

        public CustomerService(
            ICustomerRepository customerRepositoryInstance,
            ISettingService settingService)
        {
            _customerRepository = customerRepositoryInstance ?? new CustomerRepository();
            _settingService = settingService ?? new SettingService();
        }

        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            var customerInfo = _customerRepository.GetCustomerInfoByUsername(criteria);
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

                AppSession.CurrentCustomer = new SearchedCustomer
                {
                    Name = criteria.Username,
                    IsGuest = criteria.IsGuest
                };
            }
            else
            {
                AppSession.CurrentCustomer = null;
            }

            return customerInfo;
        }

        public IEnumerable<BaseCustomer> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            var uplineInfoResult = new List<BaseCustomer>();

            var rawData = _customerRepository.GetUplineInfoOfUser(criteria);
            foreach (var item in rawData)
            {
                if (criteria.CustomerId == item.Id || item.Role.Id == RoleIdConstants.Headline)
                {
                    continue;
                }

                item.Role.Name = RoleSettingService.GetRoleName(item.Role.Id);
                uplineInfoResult.Add(item);
            }

            var headline = rawData.Where(i => i.Role.Id == RoleIdConstants.Headline)
                                  .Select(i =>
                                  {
                                      i.Role.Name = RoleSettingService.GetRoleName(i.Role.Id);
                                      return i;
                                  })
                                  .FirstOrDefault() ?? NewUnAssignedCustomerInfo();
            uplineInfoResult.Insert(0, headline);

            return uplineInfoResult;
        }

        public Result UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
        {
            var result = _customerRepository.UpdateCustomerInfo(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.CustomerInfo_UpdateSuccess;
            }

            return result;
        }

        public IEnumerable<string> QueryCustomerByUsername(CustomerSuggestionCriteria criteria)
        {
            return _customerRepository.GetCustomersByUsername(criteria);
        }

        public bool UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria)
        {
            return _customerRepository.UpdateCustomerSuggestion(criteria);
        }

        public IEnumerable<BaseModel> VerifyUsername(VerifyRelatedCustomerCriteria criteria)
        {
            return _customerRepository.VerifyUsername(criteria);
        }

        private CustomerInfo NewUnAssignedCustomerInfo()
        {
            return new CustomerInfo
            {
                Name = "Unassigned",
                IsActive = false,
                IsClosed = false,
                IsSuspended = false,
                Role = new BaseModel
                {
                    Id = RoleIdConstants.Headline,
                    Name = RoleSettingService.GetRoleName(RoleIdConstants.Headline)
                }
            };
        }
    }
}
