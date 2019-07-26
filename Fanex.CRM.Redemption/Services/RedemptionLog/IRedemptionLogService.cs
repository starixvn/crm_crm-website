namespace Fanex.CRM.Redemption.Services.RedemptionLog
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;

    public interface IRedemptionLogService
    {
        CustomerLogInfo GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        LogInfo GetLogById(GetLogByIdCriteria criteria);

        CommandResult UpdateLog(UpdateLogCriteria criteria);

        CommandResult InsertLog(InsertLogCriteria criteria);

        dynamic GetAllSettingsInsert();

        dynamic GetAllSettingsEdit();
    }
}