// --------------------------------------------------------------------------
// <copyright file="ICustomerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services.Interfaces
{
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;

    public interface ICustomerService
    {
        CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria);
    }
}
