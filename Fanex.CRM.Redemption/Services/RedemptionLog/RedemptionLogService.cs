// --------------------------------------------------------------------------
// <copyright file="RedemptionLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Services.RedemptionLog
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;
    using Fanex.CRM.Redemption.Repositories.RedemptionLog;
    using Fanex.CRM.Redemption.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

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
            try
            {
                CustomerLogInfo customerInfo = null;
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                var rawData = _logRepositoryInstance.GetLogsByCustomerId(criteria);
                customerInfo = new CustomerLogInfo
                {
                    CustomerId = criteria.CustomerId,
                    Logs = rawData
                };

                return customerInfo;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the logs by identify.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The logs by running number.</returns>
        public LogInfo GetLogById(GetLogByIdCriteria criteria)
        {
            try
            {
                return _logRepositoryInstance.GetLogById(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateLog(UpdateLogCriteria criteria)
        {
            try
            {
                Result updateResult = null;
                criteria.ClaimedTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ClaimedTime);
                updateResult = _logRepositoryInstance.UpdateLog(criteria);
                if (updateResult != null && updateResult.Code == ErrorCodeConstants.NoneError)
                {
                    updateResult.Message = Labels.Redemption_Log_UpdateSuccess;
                }

                return updateResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Inserts the log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertLog(InsertLogCriteria criteria)
        {
            try
            {
                Result insertResult = null;
                criteria.ClaimedTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ClaimedTime);
                insertResult = _logRepositoryInstance.InsertLog(criteria);
                if (insertResult != null && insertResult.Code == ErrorCodeConstants.NoneError)
                {
                    insertResult.Message = Labels.Redemption_Log_InsertSuccess;
                }

                return insertResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all status for inserting.
        /// </summary>
        /// <returns>All status.</returns>
        public List<SettingInfo> AllStatusInsert()
        {
            try
            {
                var allStatusResult = new List<SettingInfo>();
                var allStatusSetting = _serviceSetting.AllStatus(new GetAllStatusCriteria
                {
                    ViewType = ViewTypeConstants.Redemption
                });

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

                Dictionary<string, int> statusIndex = new Dictionary<string, int>();
                statusIndex.Add(Labels.Open, 0);
                statusIndex.Add(Labels.Closed, 1);
                statusIndex.Add(Labels.KIV, 2);
                foreach (var status in allStatusSetting)
                {
                    if (statusIndex.ContainsKey(status.Name))
                    {
                        allStatusResult[statusIndex[status.Name]] = status;
                    }
                }

                return allStatusResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all status for editing.
        /// </summary>
        /// <returns>All status.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public List<SettingInfo> AllStatusEdit()
        {
            try
            {
                var allStatusResult = new List<SettingInfo>();
                var allStatusSetting = _serviceSetting.AllStatus(new GetAllStatusCriteria
                {
                    ViewType = ViewTypeConstants.Redemption
                });

                if (allStatusSetting == null)
                {
                    return allStatusResult;
                }

                allStatusResult = new List<SettingInfo> 
                { 
                    new SettingInfo(),
                    new SettingInfo(),
                    new SettingInfo(),
                    new SettingInfo()
                };

                Dictionary<string, int> statusIndex = new Dictionary<string, int>();
                statusIndex.Add(Labels.Open, 0);
                statusIndex.Add(Labels.Closed, 1);
                statusIndex.Add(Labels.KIV, 2);
                statusIndex.Add(Labels.Deleted, 3);

                foreach (var status in allStatusSetting)
                {
                    if (statusIndex.ContainsKey(status.Name))
                    {
                        allStatusResult[statusIndex[status.Name]] = status;
                    }
                }

                return allStatusResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}
