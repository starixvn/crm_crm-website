// --------------------------------------------------------------------------
// <copyright file="DashboardService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Dashboard.Repositories;
    using Fanex.CRM.Dashboard.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Dashboard Service.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepositoryInstance;

        public DashboardService()
            : this(null)
        {
        }

        public DashboardService(IDashboardRepository dashboardRepositoryInstance)
        {
            _dashboardRepositoryInstance = dashboardRepositoryInstance ?? new DashboardRepository();
        }

        public IEnumerable<string> GetCustomerRemarks(GetCustomerRemarksCriteria criteria)
        {
            return _dashboardRepositoryInstance.GetCustomerRemarks(criteria);
        }

        public CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            return _dashboardRepositoryInstance.GetCustomerInfo(criteria);
        }

        public IEnumerable<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            return _dashboardRepositoryInstance.GetRedemptionLogs(criteria);
        }

        public IEnumerable<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            return _dashboardRepositoryInstance.GetCommunicationLogs(criteria);
        }

        public Result UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria)
        {
            var result = _dashboardRepositoryInstance.UpdateCustomerVerifyFlag(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Dashboard_VerifyFlag_UpdateSuccess;
            }

            return result;
        }
    }
}
