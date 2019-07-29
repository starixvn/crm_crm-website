namespace Fanex.CRM.Communication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Repositories;
    using Fanex.CRM.Communication.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;

    public class CommunicationLogService : ICommunicationLogService
    {
        private readonly ICommunicationLogRepository _communicationRepositoryLog;
        private readonly ISettingService _serviceSetting;
        private readonly IBaseService baseService;

        public CommunicationLogService() : this(null, null, null)
        {
        }

        public CommunicationLogService(
            ICommunicationLogRepository communicationRepositoryInstance,
            IBaseService baseService,
            ISettingService serviceSetting)
        {
            _communicationRepositoryLog = communicationRepositoryInstance ?? new CommunicationLogRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
            this.baseService = baseService ?? new BaseService();
        }

        public IList<BaseModel> AllStatusForEdit
        {
            get
            {
                return _serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool);
            }
        }

        public IList<BaseModel> AllStatusForInsert
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
            var siteSettings = _serviceSetting.GetAllSiteWithGroup().Where(s => s.IsActive);

            var site = siteSettings.FirstOrDefault(s => s.Id == log.SiteInfo.Id);
            log.SiteInfo.Name = site?.Name;
            log.SiteInfo.AliasName = site?.AliasName;

            var licenseeType = settings.FirstOrDefault(s => s.Id == log.LicenseeType.Id);
            log.LicenseeType.Name = licenseeType == null ? Labels.Symbol_NotApply : licenseeType.Value;

            var language = settings.FirstOrDefault(s => s.Id == log.Language.Id);
            log.Language.Name = language == null ? Labels.Symbol_NotApply : language.Value;

            log.Response.Name = settings.First(s => s.Id == log.Response.Id).Value;
            log.Status.Name = settings.First(s => s.Id == log.Status.Id).Value;
            log.RoleInfo.Name = settings.First(s => s.Id == log.RoleInfo.Id).Value;
        }

        public CommandResult UpdateLog(UpdateLogCriteria criteria)
        {
            criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
            criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);

            if (!criteria.IsReviewed)
            {
                criteria.Review = string.Empty;
            }

            if (DateTime.Now < criteria.StartTime)
            {
                return new CommandResult
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = Labels.Communication_Log_ExcessCurrentTime
                };
            }

            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.Communication_Logs_UpdateSuccess;
            }

            return result;
        }

        public CommandResult InsertLog(InsertLogCriteria criteria)
        {
            criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
            criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);
            if (DateTime.Now < criteria.StartTime)
            {
                return new CommandResult
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = Labels.Communication_Log_ExcessCurrentTime
                };
            }

            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.Communication_Logs_InsertSuccess;
            }

            return result;
        }

        public CustomerDefaultSetting GetCustomerDefaultSetting(GetDefaultSettingsCriteria criteria)
        {
            return baseService.GetDataEntity<CustomerDefaultSetting>(criteria);
        }
    }
}