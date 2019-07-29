namespace Fanex.CRM.ActionLog.Services
{
    using Fanex.CRM.ActionLog.Criteria;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.Common.Models;

    public interface IActionLogService
    {
        PagedList<ActionLog> GetActivityLogs(GetActivityLogCriteria criteria);

        PagedList<SettingLog> GetSettingLogs(GetSettingLogCriteria criteria);
    }
}