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
        /// Gets the customer remarks.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer remarks.</returns>
        public List<string> GetCustomerRemarks(GetCustomerRemarksCriteria criteria)
        {
            List<string> result = null;
            try
            {
                var rawData = _repositoryInstance.GetCustomerRemarks(criteria);
                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer information.</returns>
        public CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            CustomerInfo result = null;
            try
            {
                var rawData = _repositoryInstance.GetCustomerInfo(criteria);
                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the redemption logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The redemption logs.</returns>
        public List<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            List<RedemptionLogInfo> result = null;
            try
            {
                var rawData = _repositoryInstance.GetRedemptionLogs(criteria);
                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the communication logs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The communication logs.</returns>
        public List<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            List<CommunicationLogInfo> result = null;
            try
            {
                var rawData = _repositoryInstance.GetCommunicationLogs(criteria);
                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Updates the customer verify flag.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria)
        {
            Result result = null;
            try
            {
                var rawData = _repositoryInstance.UpdateCustomerVerifyFlag(criteria);
                result = rawData;
                if (result != null &&
                    result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = Labels.Dashboard_VerifyFlag_UpdateSuccess;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}
