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

        public IList<Setting> AllStatusForEdit
        {
            get
            {
                return _serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool).ToList();
            }
        }

        /// <summary>
        /// Gets all status for insert.
        /// </summary>
        /// <value>
        /// All status for insert.
        /// </value>
        public IList<Setting> AllStatusForInsert
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
            var log = _communicationRepositoryLog.GetLogsByRunningNumber(criteria);

            UpdateSettingNameForLog(log);

            return log;
        }

        private void UpdateSettingNameForLog(BaseCommunicationLog log)
        {
            var settings = _serviceSetting.GetAllSettings();

            var licenseeType = settings.FirstOrDefault(s => s.Id == log.LicenseeType.Id);
            log.LicenseeType.Name = licenseeType == null ? null : licenseeType.Value;

            var communication = settings.FirstOrDefault(s => s.Id == log.CommunicationTool.Id);
            log.CommunicationTool.Name = communication == null ? null : communication.Value;

            var product = settings.FirstOrDefault(s => s.Id == log.Product.Id);
            log.Product.Name = product == null ? null : product.Value;

            var language = settings.FirstOrDefault(s => s.Id == log.Language.Id);
            log.Language.Name = language == null ? null : language.Value;

            var bettype = settings.FirstOrDefault(s => s.Id == log.BetType.Id);
            log.BetType.Name = bettype == null ? null : bettype.Value;

            log.Response.Name = settings.First(s => s.Id == log.Response.Id).Value;
            log.MainCategory.Name = settings.First(s => s.Id == log.MainCategory.Id).Value;
            log.Status.Name = settings.First(s => s.Id == log.Status.Id).Value;
            log.Extension.Name = settings.First(s => s.Id == log.Extension.Id).Value;
            log.RoleInfo.Name = settings.First(s => s.Id == log.RoleInfo.Id).Value;
            log.SubCategory.Name = settings.First(s => s.Id == log.SubCategory.Id).Value;
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