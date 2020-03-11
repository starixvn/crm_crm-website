namespace Fanex.CRM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Criteria.MultipleLogs;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.MultipleLogs;
    using Fanex.CRM.Repositories;
    using Fanex.CRM.Resources;
    using Fanex.CRM.Setting.Services;

    public class MultipleLogsService : IMultipleLogsService
    {
        private readonly IMultipleLogsRepository multipleLogsRepository;
        private readonly ISettingService serviceSetting;
        private readonly IBaseService baseService;

        public MultipleLogsService() : this(null, null, null)
        {
        }

        public MultipleLogsService(
            IMultipleLogsRepository multipleLogsRepository,
            IBaseService baseService,
            ISettingService serviceSetting)
        {
            this.multipleLogsRepository = multipleLogsRepository ?? new MultipleLogsRepository();
            this.serviceSetting = serviceSetting ?? new SettingService();
            this.baseService = baseService ?? new BaseService();
        }

        public IEnumerable<VerifyCustomerResult> VerifyCustomer(VerifyCustomerCriteria criteria)
        {
            return baseService.GetData<VerifyCustomerResult>(criteria);
        }

        public IEnumerable<VerifyRelatedCustomerResult> VerifyRelatedCustomer(VerifyRelatedCustomerCriteria criteria)
        {
            return baseService.GetData<VerifyRelatedCustomerResult>(criteria);
        }

        public IEnumerable<VerifyCustomer> CheckNotExistCommunicationToolSetting(CheckNotExistCommunicationToolSettingCriteria criteria)
        {
            return baseService.GetData<VerifyCustomer>(criteria);
        }

        public CommandResult Create(CreateMultipleLogsCriteria criteria)
        {
            criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
            criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);
            if (DateTime.Now < criteria.StartTime)
            {
                return new CommandResult
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = Messages.StartTimeShouldBeSmallerThanOrEqualCurrentTime
                };
            }

            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Messages.CreateMultipleLogsSuccessfully;
            }

            return result;
        }

        public CommandResult Update(UpdateMultipleLogsCriteria criteria)
        {
            criteria.StartTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.StartTime);
            criteria.EndTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.EndTime);
            if (DateTime.Now < criteria.StartTime)
            {
                return new CommandResult
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = Messages.StartTimeShouldBeSmallerThanOrEqualCurrentTime
                };
            }

            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Messages.UpdateMultipleLogsSuccessfully;
            }

            return result;
        }

        public CommandResult UpdateMultipleLogStatus(UpdateMultipleLogStatusCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Messages.UpdateMultipleLogsSuccessfully;
            }

            return result;
        }

        public BaseCommunicationLog GetLogByFeedbackId(GetLogByFeedbackIdCriteria criteria)
        {
            var log = multipleLogsRepository.GetLogByFeedbackId(criteria);
            UpdateSettingNameForLog(log);

            return log;
        }

        private void UpdateSettingNameForLog(BaseCommunicationLog log)
        {
            var settings = serviceSetting.GetAllSettings();
            var siteSettings = serviceSetting.GetAllSiteWithGroup().Where(s => s.IsActive);

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
    }
}