// --------------------------------------------------------------------------
// <copyright file="IDashboardRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Repositories.Interfaces
{
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;

    public interface IDashboardRepository
    {
        CustomerDashboardInfo GetCustomerDashboardInfo(GetCustomerDashboardInfoCriteria criteria);
    }
}
