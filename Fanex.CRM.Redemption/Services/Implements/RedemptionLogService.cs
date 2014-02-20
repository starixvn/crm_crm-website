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
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;
    using Fanex.CRM.Redemption.Repositories.Implements;
    using Fanex.CRM.Redemption.Repositories.Interfaces;
    using Fanex.CRM.Redemption.Resources;
    using Fanex.CRM.Redemption.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Redemption Log Service.
    /// </summary>
    public class RedemptionLogService : IRedemptionLogService
    {
        private readonly IRedemptionLogRepository _logRepositoryInstance;

        private ISettingService _serviceSetting;

        public RedemptionLogService()
            : this(null, null)
        {
        }

        public RedemptionLogService(
            IRedemptionLogRepository logRepositoryInstance,
            ISettingService serviceSetting)
        {
            _logRepositoryInstance = logRepositoryInstance ?? new RedemptionLogRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        /// <summary>
        /// Gets the logs by customer identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by customer identifier.</returns>
        public CustomerLogInfo GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            CustomerLogInfo customerInfo = null;
            try
            {
                var rawData = _logRepositoryInstance.GetLogsByCustomerId(criteria);
                customerInfo = new CustomerLogInfo
                {
                    CustomerId = criteria.CustomerId,
                    Logs = rawData
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return customerInfo;
        }

        /// <summary>
        /// Gets the logs by identify.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by running number.</returns>
        public LogInfo GetLogById(GetLogByIdCriteria criteria)
        {
            LogInfo log = null;
            try
            {
                var rawData = _logRepositoryInstance.GetLogById(criteria);
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
                criteria.ClaimedTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ClaimedTime);
                updateResult = _logRepositoryInstance.UpdateLog(criteria);
                if (updateResult != null && updateResult.Code == ErrorCodeConstants.NoneError)
                {
                    updateResult.Message = Labels.Redemption_Log_UpdateSuccess;
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
                criteria.ClaimedTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ClaimedTime);
                insertResult = _logRepositoryInstance.InsertLog(criteria);
                if (insertResult != null && insertResult.Code == ErrorCodeConstants.NoneError)
                {
                    insertResult.Message = Labels.Redemption_Log_InsertSuccess;
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
        public List<SettingInfo> GetAllStatus()
        {
            var allStatusResult = new List<SettingInfo>();
            try
            {
                var allStatusSetting = _serviceSetting.GetAllStatus();
                if (allStatusSetting == null)
                {
                    return allStatusResult;
                }

                allStatusResult = new List<SettingInfo> 
                { 
                    new SettingInfo(),
                    new SettingInfo(),
                    new SettingInfo()
                };

                foreach (var status in allStatusSetting)
                {
                    if (status.Name == Labels.Open)
                    {
                        allStatusResult[0] = status;
                        continue;
                    }

                    if (status.Name == Labels.Closed)
                    {
                        allStatusResult[1] = status;
                        continue;
                    }

                    if (status.Name == Labels.KIV)
                    {
                        allStatusResult[2] = status;
                        continue;
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
