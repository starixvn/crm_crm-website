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
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories.Implements;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Customer.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Extension;
    using System.Web;
    using Fanex.CRM.Infrastructure.Common;

    /// <summary>
    /// Customer Service.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repositoryInstance;

        public CustomerService()
            : this(null)
        {
        }

        public CustomerService(ICustomerRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new CustomerRepository();
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
                    //result.Site = _settingService.GetById(result.Site.Id);
                }

                SessionExtension.AddSession(HttpContext.Current.Session, FunctionPermission.SearchCustomerInfo, criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}
