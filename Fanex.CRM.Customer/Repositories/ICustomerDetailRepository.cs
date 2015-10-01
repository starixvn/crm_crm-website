// --------------------------------------------------------------------------
// <copyright file="ICustomerDetailRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Repositories
{
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface ICustomerDetailRepository
    {
        CustomerInfo GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria);

        Result UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria);
    }
}
