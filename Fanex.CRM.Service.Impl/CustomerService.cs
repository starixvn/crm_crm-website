// --------------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using Fanex.CRM.DataAccess.Impl;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Interface;
    using MongoDB.Driver;

    /// <summary>
    /// Customer service.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private const string KeyCustomer = "_customerKey";
        private readonly ICustomerDataAccess _repository;
        private readonly IPermissionService _permissionService;
        private User _loginUser;      

        public CustomerService() : this(null, null) 
        { 
        }

        public CustomerService(ICustomerDataAccess repository, IPermissionService permissionService)
        {
            this._repository = repository ?? new CustomerDataAccess();
            this._permissionService = permissionService ?? new PermissionService();
            this._loginUser = SessionExtension.GetSession(HttpContext.Current.Session, "CRMUserInfo") as User;
        }

        /// <summary>
        /// Getting customer info by id.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <returns>Customer info.</returns>
        public Customer GetCustomerInfoById(int custId)
        {
            try
            {
                var customer = _repository.FindBy(custId);
                return customer;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Customer();
            }
        }

        /// <summary>
        /// Verify username.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>Related customers.</returns>
        public List<RelatedCustomer> VerifyUserName(string userName)
        {
            try
            {
                var result = this._repository.VerifyUserName(userName);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<RelatedCustomer>();
            }
        }

        /// <summary>
        /// Get customer info by username.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="isGuest">Is guest customer.</param>
        /// <returns>Customer info.</returns>
        public Customer GetCustomerInfoByUserName(string userName, bool isGuest)
        {
            try
            {
                HttpContext.Current.Session.Remove(FunctionPermission.SearchCustomerInfo);

                var customer = this._repository.FindBy(userName, isGuest);
                return customer;
            }
            catch (Exception ex)
            {                
                LogExtensions.ExceptionHandler(ex);
                return new Customer();
            }
        }

        /// <summary>
        /// Getting up-line list by customer id.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <returns>Up-line list.</returns>
        public List<Customer> GetListUpline(int custId)
        {
            try
            {
                List<Customer> listUpline = this._repository.FindAllUpline(custId);
                return listUpline;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<Customer>();
            }
        }

        /// <summary>
        /// Updating customer info.
        /// </summary>
        /// <param name="customer">Customer info.</param>
        /// <param name="isUpdateUserName">Is updating username.</param>
        /// <param name="ip">User IP address.</param>
        /// <returns>Return result.</returns>
        public Result UpdateCustomerInfo(Customer customer, bool isUpdateUserName, string ip)
        {
            var result = new Result();
            try
            {
                if (!this._permissionService.HasHostPermission(FunctionPermission.CustomerInfo.ToString()))
                {
                    result.ErrCode = 500;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoUpdateCustomerPermission");
                    return result;
                }

                this._repository.UpdateCustomerInfo(customer, isUpdateUserName, ip, ref result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
        
        /// <summary>
        /// Getting auto search.
        /// </summary>
        /// <param name="userID">Customer id.</param>
        /// <param name="userName">Customer name.</param>
        /// <param name="isGuest">Is guest customer.</param>
        /// <returns>Return result.</returns>
        public System.Data.DataTable GetAutoSearch(int userID, string userName, bool isGuest)
        {
            var result = new DataTable();
            try
            {
                var getData = this._repository.QueryUserName(userID, userName, isGuest);
                if (getData != null && getData.Tables.Count > 0)
                {
                    result = getData.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
                
        /// <summary>
        /// Updating auto customer search.
        /// </summary>
        /// <param name="custID">Customer id who is logging.</param>
        /// <param name="userID">User id who is updated.</param>
        /// <param name="userName">The name of user who is updated.</param>
        /// <returns>Return result.</returns>
        public Result UpdateAutoCustomerSearch(int custID, int userID, string userName)
        {
            var result = new Result();
            try
            {
                if (!this._permissionService.HasHostPermission(FunctionPermission.CustomerInfo.ToString()))
                {
                    result.ErrCode = 500;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoUpdateCustomerPermission");
                    return result;
                }

                if (userID > 0)
                {
                    this._repository.UpdateCustomerSearch(custID, userID, userName, ref result);
                }
                else if (userID < 0)
                {
                    UpdateCustomerSearchCountValue(userID, userName);
                }                    
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Customer auto suggestion.
        /// </summary>
        /// <param name="customerName">Customer name.</param>
        /// <param name="isGuest">Is guest customer.</param>
        /// <returns>Return result.</returns>
        public MongoCursor<MongoDB.Bson.BsonDocument> CustomerAutoSuggest(string customerName, bool isGuest)
        {
            MongoCursor<MongoDB.Bson.BsonDocument> result = null;
            try
            {
                result = this._repository.CustomerAutoSuggest(customerName.ToLower(), isGuest);
            }
            catch (Exception ex)
            {               
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Updating customer search count value.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <param name="customerName">Customer name.</param>
        /// <returns>Return result.</returns>
        public bool UpdateCustomerSearchCountValue(int custId, string customerName)
        {
            bool result = false;
            try
            {
                result = this._repository.UpdateCustomerSearchCountValue(custId, customerName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}
