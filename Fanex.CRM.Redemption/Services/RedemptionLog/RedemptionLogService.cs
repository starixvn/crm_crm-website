namespace Fanex.CRM.Redemption.Services.RedemptionLog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;
    using Fanex.CRM.Redemption.Repositories.RedemptionLog;
    using Fanex.CRM.Redemption.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    public class RedemptionLogService : IRedemptionLogService
    {
        private readonly IRedemptionLogRepository _logRepositoryInstance;
        private readonly ISettingService _serviceSetting;
        private readonly IBaseService baseService;

        public RedemptionLogService() : this(null, null, null)
        {
        }

        public RedemptionLogService(
            IRedemptionLogRepository logRepositoryInstance,
            IBaseService baseService,
            ISettingService serviceSetting)
        {
            _logRepositoryInstance = logRepositoryInstance ?? new RedemptionLogRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
            this.baseService = baseService ?? new BaseService();
        }

        private IEnumerable<BaseModel> GetRedemptionCurrencies()
        {
            var currencies = _serviceSetting.GetAllCurrencyRedemptions(new GetAllCurrencyRedemptionCriteria());
            currencies.Insert(0, new BaseModel { Id = SettingConstants.NotApply, Name = Labels.Symbol_NotApply });

            return currencies;
        }

        public CustomerLogInfo GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            var logs = _logRepositoryInstance.GetLogsByCustomerId(criteria);
            var customerInfo = new CustomerLogInfo
            {
                CustomerId = criteria.CustomerId,
                Logs = logs
            };

            return customerInfo;
        }

        public LogInfo GetLogById(GetLogByIdCriteria criteria)
        {
            return _logRepositoryInstance.GetLogById(criteria);
        }

        public CommandResult UpdateLog(UpdateLogCriteria criteria)
        {
            criteria.ClaimedTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ClaimedTime);
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.Redemption_Log_UpdateSuccess;
            }

            return result;
        }

        public CommandResult InsertLog(InsertLogCriteria criteria)
        {
            criteria.ClaimedTime = DateTimeUtilize.ChangeTimeZoneToServerDateTime(criteria.ClaimedTime);
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.Redemption_Log_InsertSuccess;
            }

            return result;
        }

        public dynamic GetAllSettingsInsert()
        {
            var statusList = _serviceSetting.GetAllStatuses(ViewTypeConstants.Redemption)
                .Where(s => !string.Equals(s.Name, Labels.Deleted, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return new
            {
                StatusList = statusList,
                Currencies = GetRedemptionCurrencies()
            };
        }

        public dynamic GetAllSettingsEdit()
        {
            var statusList = _serviceSetting.GetAllStatuses(ViewTypeConstants.Redemption);

            return new
            {
                StatusList = statusList,
                Currencies = GetRedemptionCurrencies()
            };
        }
    }
}