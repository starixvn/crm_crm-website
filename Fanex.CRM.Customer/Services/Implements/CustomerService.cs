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
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
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

        /// <summary>
        /// Gets the customer information by username.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer information by username.</returns>
        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            CustomerInfo customerInfo = null;

            try
            {
                var rawData = _customerRepositoryInstance.GetCustomerInfoByUsername(criteria);
                customerInfo = rawData;
                if (customerInfo != null)
                {
                    customerInfo.Role.Name = RoleSettingService.GetRoleName(customerInfo.Role.Id);
                    var siteSetting = _settingService.SettingById(new GetSettingByIdCriteria
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
                    SessionExtension.AddSession(HttpContext.Current.Session, GetCustomerInfoByUsernameSessionKey, null);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return customerInfo;
        }

        /// <summary>
        /// Gets the up line information of user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The up line information of user.</returns>
        public List<BaseCustomerInfo> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            List<BaseCustomerInfo> uplineInfoResult = new List<BaseCustomerInfo>();
            try
            {
                var rawData = _customerRepositoryInstance.GetUplineInfoOfUser(criteria);
                for (var i = 0; i < rawData.Count; i++)
                {
                    var uplineInfo = rawData[i];
                    if (criteria.CustomerId == uplineInfo.Id)
                    {
                        continue;
                    }

                    uplineInfo.Role.Name = RoleSettingService.GetRoleName(uplineInfo.Role.Id);
                    uplineInfoResult.Add(uplineInfo);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return uplineInfoResult;
        }

        /// <summary>
        /// Updates the customer information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public BaseModel UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
        {
            var updateResult = new BaseModel();
            try
            {
                updateResult = _customerRepositoryInstance.UpdateCustomerInfo(criteria);
                if (updateResult != null &&
                    updateResult.Id == ErrorCodeConstants.NoneError)
                {
                    updateResult.Name = Labels.CustomerInfo_UpdateSuccess;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return updateResult;
        }

        /// <summary>
        /// Queries the customer by username.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of Customer.</returns>
        public IList<string> QueryCustomerByUsername(CustomerSuggestionCriteria criteria)
        {
            try
            {
                return _customerRepositoryInstance.QueryCustomerByUsername(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the customer suggestion.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public bool UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria)
        {
            var updateResult = false;
            try
            {
                updateResult = _customerRepositoryInstance.UpdateCustomerSuggestion(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return updateResult;
        }

        /// <summary>
        /// Suggests related customer.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Related customers.</returns>
        public List<dynamic> RelatedCustomerAutoSuggestion(RelatedCustomerAutoSuggestionCriteria criteria)
        {
            try
            {
                var dtoResult = new List<dynamic>();
                var customerList = _customerRepositoryInstance.RelatedCustomerAutoSuggestion(criteria);
                for (var i = 0; i < customerList.Count; i++)
                {
                    var customer = customerList[i];
                    dtoResult.Add(new
                    {
                        key = customer.Key,
                        value = customer.Value
                    });
                }

                return dtoResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Verifies the username.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Verified customer information.</returns>
        public List<CustomerInfo> VerifyUsername(VerifyRelatedCustomerCriteria criteria)
        {
            try
            {
                return _customerRepositoryInstance.VerifyUsername(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}
