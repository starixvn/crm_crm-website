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
    using System.Linq;
    using Fanex.CRM.Common.Helpers;
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
        public dynamic AllSettingsInsert()
        {
            try
            {
                var statusList = _serviceSetting.AllStatus(ViewTypeConstants.Redemption);
                return new
                {
                    StatusList = statusList,
                    Currencies = _serviceSetting.GetAllCurrency(new GetAllCurrencyCriteria())
                };
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
        public dynamic AllSettingsEdit()
        {
            try
            {
                var statusList = _serviceSetting.AllStatus(ViewTypeConstants.Redemption);

                return new
                {
                    StatusList = statusList,
                    Currencies = _serviceSetting.GetAllCurrency(new GetAllCurrencyCriteria())
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}
