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
    using System.Web;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Repositories.Implements;
    using Fanex.CRM.Communication.Repositories.Interfaces;
    using Fanex.CRM.Communication.Resources;
    using Fanex.CRM.Communication.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;

    /// <summary>
    /// Communication Log Service.
    /// </summary>
    public class CommunicationLogService : ICommunicationLogService
    {
        private const string GetLogsByCustomerIdSessionKey = "Fanex.CRM.Customer.Services.Implements.CustomerService.GetLogsByCustomerIdSessionKey";

        private readonly ICommunicationLogRepository _repositoryInstance;

        public CommunicationLogService()
            : this(null)
        {
        }

        public CommunicationLogService(ICommunicationLogRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new CommunicationLogRepository();
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

                SessionExtension.AddSession(HttpContext.Current.Session, GetLogsByCustomerIdSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the logs by running number.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by running number.</returns>
        public LogInfo GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria)
        {
            LogInfo result = null;
            try
            {
                var rawData = _repositoryInstance.GetLogsByRunningNumber(criteria);
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
                    result.Message = Labels.Communication_Logs_UpdateSuccess;
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
                    result.Message = Labels.Communication_Logs_InsertSuccess;
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
