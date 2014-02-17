// --------------------------------------------------------------------------
// <copyright file="CommunicationLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Services.Implements
{
    using System;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Repositories.Implements;
    using Fanex.CRM.Communication.Repositories.Interfaces;
    using Fanex.CRM.Communication.Resources;
    using Fanex.CRM.Communication.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;

    /// <summary>
    /// Communication Log Service.
    /// </summary>
    public class CommunicationLogService : ICommunicationLogService
    {
        private readonly ICommunicationLogRepository _repositoryLog;

        public CommunicationLogService()
            : this(null)
        {
        }

        public CommunicationLogService(ICommunicationLogRepository repositoryInstance)
        {
            _repositoryLog = repositoryInstance ?? new CommunicationLogRepository();
        }

        /// <summary>
        /// Gets the logs by customer identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by customer identifier.</returns>
        public CustomerLogInfo GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            CustomerLogInfo logs = null;
            try
            {
                var rawData = _repositoryLog.GetLogsByCustomerId(criteria);
                logs = new CustomerLogInfo
                {
                    CustomerId = criteria.CustomerId,
                    Logs = rawData
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return logs;
        }

        /// <summary>
        /// Gets the logs by running number.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by running number.</returns>
        public LogInfo GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria)
        {
            LogInfo log = null;
            try
            {
                var rawData = _repositoryLog.GetLogsByRunningNumber(criteria);
                log = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return log;
        }

        /// <summary>
        /// Updates the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateLog(UpdateLogCriteria criteria)
        {
            Result updateResult = null;
            try
            {
                updateResult = _repositoryLog.UpdateLog(criteria);
                if (updateResult != null && updateResult.Code == ErrorCodeConstants.NoneError)
                {
                    updateResult.Message = Labels.Communication_Logs_UpdateSuccess;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return updateResult;
        }

        /// <summary>
        /// Inserts the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertLog(InsertLogCriteria criteria)
        {
            Result insertResult = null;
            try
            {
                insertResult = _repositoryLog.InsertLog(criteria);
                if (insertResult != null && insertResult.Code == ErrorCodeConstants.NoneError)
                {
                    insertResult.Message = Labels.Communication_Logs_InsertSuccess;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return insertResult;
        }
    }
}
