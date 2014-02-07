// --------------------------------------------------------------------------
// <copyright file="RedemptionLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Services.Implements
{
    using System;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;
    using Fanex.CRM.Redemption.Repositories.Implements;
    using Fanex.CRM.Redemption.Repositories.Interfaces;
    using Fanex.CRM.Redemption.Resources;
    using Fanex.CRM.Redemption.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;

    /// <summary>
    /// Redemption Log Service.
    /// </summary>
    public class RedemptionLogService : IRedemptionLogService
    {
        private readonly IRedemptionLogRepository _repositoryInstance;

        public RedemptionLogService()
            : this(null)
        {
        }

        public RedemptionLogService(IRedemptionLogRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new RedemptionLogRepository();
        }

        /// <summary>
        /// Gets the logs by customer identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by customer identifier.</returns>
        public CustomerLogInfo GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            CustomerLogInfo result = null;
            try
            {
                var rawData = _repositoryInstance.GetLogsByCustomerId(criteria);
                result = new CustomerLogInfo
                {
                    CustomerId = criteria.CustomerId,
                    Logs = rawData
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the logs by identify.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by running number.</returns>
        public LogInfo GetLogById(GetLogByIdCriteria criteria)
        {
            LogInfo result = null;
            try
            {
                var rawData = _repositoryInstance.GetLogById(criteria);
                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Updates the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateLog(UpdateLogCriteria criteria)
        {
            Result result = null;
            try
            {
                result = _repositoryInstance.UpdateLog(criteria);
                if (result != null && result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = Labels.Redemption_Log_UpdateSuccess;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Inserts the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertLog(InsertLogCriteria criteria)
        {
            Result result = null;
            try
            {
                result = _repositoryInstance.InsertLog(criteria);
                if (result != null && result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = Labels.Redemption_Log_InsertSuccess;
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
