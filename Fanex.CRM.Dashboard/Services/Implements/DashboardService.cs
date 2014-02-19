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
    using System.Collections.Generic;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Dashboard.Repositories.Implements;
    using Fanex.CRM.Dashboard.Repositories.Interfaces;
    using Fanex.CRM.Dashboard.Resources;
    using Fanex.CRM.Dashboard.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;

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

        /// <summary>
        /// Gets the customer remarks.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer remarks.</returns>
        public List<string> GetCustomerRemarks(GetCustomerRemarksCriteria criteria)
        {
            List<string> remarks = null;
            try
            {
                var rawData = _dashboardRepositoryInstance.GetCustomerRemarks(criteria);
                remarks = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return remarks;
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer information.</returns>
        public CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            CustomerInfo customerInfo = null;
            try
            {
                var rawData = _dashboardRepositoryInstance.GetCustomerInfo(criteria);
                customerInfo = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return customerInfo;
        }

        /// <summary>
        /// Gets the redemption logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The redemption logs.</returns>
        public List<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            List<RedemptionLogInfo> redemptionLogs = null;
            try
            {
                var rawData = _dashboardRepositoryInstance.GetRedemptionLogs(criteria);
                redemptionLogs = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return redemptionLogs;
        }

        /// <summary>
        /// Gets the communication logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The communication logs.</returns>
        public List<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            List<CommunicationLogInfo> communicationLogs = null;
            try
            {
                var rawData = _dashboardRepositoryInstance.GetCommunicationLogs(criteria);
                communicationLogs = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return communicationLogs;
        }

        /// <summary>
        /// Updates the customer verify flag.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria)
        {
            Result updateResult = null;
            try
            {
                var rawData = _dashboardRepositoryInstance.UpdateCustomerVerifyFlag(criteria);
                updateResult = rawData;
                if (updateResult != null &&
                    updateResult.Code == ErrorCodeConstants.NoneError)
                {
                    updateResult.Message = Labels.Dashboard_VerifyFlag_UpdateSuccess;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return updateResult;
        }
    }
}
