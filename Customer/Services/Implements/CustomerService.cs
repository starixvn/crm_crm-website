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
    using Fanex.CRM.Customer.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Customer.Models;

    /// <summary>
    /// Customer Service.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }
    }
}
