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
    using System.Collections.Generic;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Repositories.Implements;
    using Fanex.CRM.Communication.Repositories.Interfaces;
    using Fanex.CRM.Communication.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Extension;
    using System.Web;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Communication.Resources;

    /// <summary>
    /// Communication Log Service.
    /// </summary>
    public class CommunicationLogService : ICommunicationLogService
    {
        private readonly ICommunicationLogRepository _repositoryInstance;

        private readonly string GetLogsByCustomerIdSessionKey = "Fanex.CRM.Customer.Services.Implements.CustomerService.GetLogsByCustomerIdSessionKey";

        public CommunicationLogService()
            : this(null)
        {
        }

        public CommunicationLogService(ICommunicationLogRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new CommunicationLogRepository();
        }

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

        public LogInfo GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria)
        {
            LogInfo result = null;
            try
            {
                CustomerLogInfo storedLogs = (CustomerLogInfo)SessionExtension.GetSession(
                    HttpContext.Current.Session,
                    GetLogsByCustomerIdSessionKey);
                if (storedLogs == null)
                {
                    return result;
                }

                foreach (var log in storedLogs.Logs)
                {
                    if (log.RunningNumber == criteria.RunningNumber)
                    {
                        result = log;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

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
