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
    using Fanex.CRM.Communication.Resources;
    using Fanex.CRM.Communication.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Communication Log Service.
    /// </summary>
    public class CommunicationLogService : ICommunicationLogService
    {
        private readonly ICommunicationLogRepository _communicationRepositoryLog;

        private ISettingService _serviceSetting;

        public CommunicationLogService()
            : this(null, null)
        {
        }

        public CommunicationLogService(
            ICommunicationLogRepository communicationRepositoryInstance,
            ISettingService serviceSetting)
        {
            _communicationRepositoryLog = communicationRepositoryInstance ?? new CommunicationLogRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
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
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                var rawData = _communicationRepositoryLog.GetLogsByCustomerId(criteria);
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
                var rawData = _communicationRepositoryLog.GetLogsByRunningNumber(criteria);
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
                criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
                criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);

                if (!criteria.IsReviewed)
                {
                    criteria.Review = string.Empty;
                }

                updateResult = _communicationRepositoryLog.UpdateLog(criteria);

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
                criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
                criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);
                insertResult = _communicationRepositoryLog.InsertLog(criteria);
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

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>All status.</returns>
        public List<SettingInfo> GetAllStatus_Edit()
        {
            var allStatusResult = new List<SettingInfo>();
            try
            {
                var allStatusSetting = _serviceSetting.GetAllStatus(new GetAllStatusCriteria
                {
                    ViewType = ViewTypeConstants.CommunicationTool
                });

                if (allStatusSetting == null)
                {
                    return allStatusResult;
                }

                foreach (var status in allStatusSetting)
                {
                    if (status.Name == Labels.Checked ||
                        status.Name == Labels.Solved ||
                        status.Name == Labels.Closed ||
                        status.Name == Labels.Pending)
                    {
                        allStatusResult.Add(status);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allStatusResult;
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>
        /// All status.
        /// </returns>
        public List<SettingInfo> GetAllStatus_Insert()
        {
            var allStatusResult = new List<SettingInfo>();
            try
            {
                var allStatusSetting = _serviceSetting.GetAllStatus(new GetAllStatusCriteria
                {
                    ViewType = ViewTypeConstants.CommunicationTool
                });

                if (allStatusSetting == null)
                {
                    return allStatusResult;
                }

                foreach (var status in allStatusSetting)
                {
                    if (status.Name == Labels.Checked ||
                        status.Name == Labels.Solved ||
                        status.Name == Labels.Closed ||
                        status.Name == Labels.Pending)
                    {
                        allStatusResult.Add(status);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allStatusResult;
        }
    }
}
