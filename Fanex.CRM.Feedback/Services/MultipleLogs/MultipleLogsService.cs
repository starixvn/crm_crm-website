namespace Fanex.CRM.Feedback.Services
{
    using Common.Helpers;
    using Common.Models;
    using Criteria;
    using Infrastructure.Constants;
    using Infrastructure.Models;
    using Repositories;
    using Resources;
    using Setting.Constants;
    using Setting.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MultipleLogsService : IMultipleLogsService
    {
        private readonly IMultipleLogsRepository _multipleLogsRepository;

        private ISettingService _serviceSetting;

        public MultipleLogsService()
            : this(null, null)
        {
        }

        public MultipleLogsService(
            IMultipleLogsRepository multipleLogsRepository,
            ISettingService serviceSetting)
        {
            _multipleLogsRepository = multipleLogsRepository ?? new MultipleLogsRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        public IList<BaseModel> AllStatusForInsert
        {
            get
            {
                return AllStatusForEdit.Where(s => !string.Equals(s.Name, Labels.Deleted, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public IList<BaseModel> AllStatusForEdit
        {
            get
            {
                return _serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool);
            }
        }

        public Result InsertMultpleLogs(InsertMultipleLogsCriteria criteria)
        {
            criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
            criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);
            if (DateTime.Now < criteria.StartTime)
            {
                return new Result
                {
                    Code = ErrorCodeConstants.ServerError,
                    Message = Labels.Multiple_Logs_ExcessCurrentTime
                };
            }

            var result = _multipleLogsRepository.InsertMultipleLogs(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Multiple_Logs_InsertSuccess;
            }

            return result;
        }

        public Result UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria)
        {
            criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
            criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);
            if (DateTime.Now < criteria.StartTime)
            {
                return new Result
                {
                    Code = ErrorCodeConstants.ServerError,
                    Message = Labels.Multiple_Logs_ExcessCurrentTime
                };
            }

            var result = _multipleLogsRepository.UpdateMultipleLogs(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Multiple_Logs_UpdateSuccess;
            }

            return result;
        }

        public Result UpdateMultipleLogStatuses(UpdateMultipleLogStatusesCriteria criteria)
        {
            var result = _multipleLogsRepository.UpdateMultipleLogStatuses(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Multiple_Logs_UpdateSuccess;
            }

            return result;
        }

        public BaseCommunicationLog GetLogByFeedbackId(GetLogByFeedbackIdCriteria criteria)
        {
            var log = _multipleLogsRepository.GetLogByFeedbackId(criteria);

            UpdateSettingNameForLog(log);

            return log;
        }

        private void UpdateSettingNameForLog(BaseCommunicationLog log)
        {
            var settings = _serviceSetting.GetAllSettings();
            var siteSettings = _serviceSetting.GetAllSiteWithGroup().Where(s => s.IsActive);

            var site = siteSettings.FirstOrDefault(s => s.Id == log.SiteInfo.Id);
            log.SiteInfo.Name = site == null ? null : site.Name;
            log.SiteInfo.AliasName = site == null ? null : site.AliasName;

            var licenseeType = settings.FirstOrDefault(s => s.Id == log.LicenseeType.Id);
            log.LicenseeType.Name = licenseeType == null ? Labels.Symbol_NotApply : licenseeType.Value;

            var language = settings.FirstOrDefault(s => s.Id == log.Language.Id);
            log.Language.Name = language == null ? Labels.Symbol_NotApply : language.Value;

            log.Response.Name = settings.First(s => s.Id == log.Response.Id).Value;
            log.Status.Name = settings.First(s => s.Id == log.Status.Id).Value;
            log.RoleInfo.Name = settings.First(s => s.Id == log.RoleInfo.Id).Value;
        }
    }
}
