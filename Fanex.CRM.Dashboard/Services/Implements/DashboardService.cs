// --------------------------------------------------------------------------
// <copyright file="DashboardService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Services.Implements
{
    using System;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Dashboard.Repositories.Implements;
    using Fanex.CRM.Dashboard.Repositories.Interfaces;
    using Fanex.CRM.Dashboard.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Logging;

    /// <summary>
    /// Dashboard Service.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repositoryInstance;

        public DashboardService()
            : this(null)
        {
        }

        public DashboardService(IDashboardRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new DashboardRepository();
        }

        /// <summary>
        /// Gets the customer dashboard information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer dashboard information.</returns>
        public CustomerDashboardInfo GetCustomerDashboardInfo(GetCustomerDashboardInfoCriteria criteria)
        {
            CustomerDashboardInfo result = null;
            try
            {
                var rawData = _repositoryInstance.GetCustomerDashboardInfo(criteria);
                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}
