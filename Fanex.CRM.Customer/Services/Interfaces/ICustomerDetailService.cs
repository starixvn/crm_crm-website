// --------------------------------------------------------------------------
// <copyright file="ICustomerDetailService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services
{
    using Common.Enumerations;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface ICustomerDetailService
    {
        CustomerInfo GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria);

        Result UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria);

        dynamic GetAllSettings(UserRole userRole);
    }
}
