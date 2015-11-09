// --------------------------------------------------------------------------
// <copyright file="CommunicationLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Repositories;
    using Fanex.CRM.Communication.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;

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

        public List<SettingInfo> AllStatusForEdit
        {
            return _serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool).ToList();
		}

        /// <summary>
        /// Gets all status for insert.
        /// </summary>
        /// <value>
        /// All status for insert.
        /// </value>
        public List<SettingInfo> AllStatusForInsert
        {
            get
            {
                return AllStatusForEdit.Where(s => !string.Equals(s.Name, Labels.Deleted, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public CustomerLogInfo GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            var rawData = _communicationRepositoryLog.GetLogsByCustomerId(criteria);
            return new CustomerLogInfo
            {
                CustomerId = criteria.CustomerId,
                Logs = rawData
            };
        }

        public BaseCommunicationLog GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria)
        {
            return _communicationRepositoryLog.GetLogsByRunningNumber(criteria);
        }

        public Result UpdateLog(UpdateLogCriteria criteria)
        {
            criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
            criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);

            if (!criteria.IsReviewed)
            {
                criteria.Review = string.Empty;
            }

            if (DateTime.Now < criteria.StartTime)
            {
                return new Result
                {
                    Code = ErrorCodeConstants.ServerError,
                    Message = Labels.Communication_Log_ExcessCurrentTime
                };
            }

            var result = _communicationRepositoryLog.UpdateLog(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Communication_Logs_UpdateSuccess;
            }

            return result;
        }

        public Result InsertLog(InsertLogCriteria criteria)
        {
            criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
            criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);
            if (DateTime.Now < criteria.StartTime)
            {
                return new Result
                {
                    Code = ErrorCodeConstants.ServerError,
                    Message = Labels.Communication_Log_ExcessCurrentTime
                };
            }

            var result = _communicationRepositoryLog.InsertLog(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Communication_Logs_InsertSuccess;
            }

            return result;
        }

        public CustomerDefaultSetting GetCustomerDefaultSetting(GetDefaultSettingsCriteria criteria)
        {
            return _communicationRepositoryLog.GetCustomerDefaultSetting(criteria);
        }
    }
}
