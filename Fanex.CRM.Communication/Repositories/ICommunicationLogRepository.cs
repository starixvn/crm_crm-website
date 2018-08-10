namespace Fanex.CRM.Communication.Repositories
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Infrastructure.Models;
    using System.Collections.Generic;

    public interface ICommunicationLogRepository
    {
        IEnumerable<BaseCommunicationLog> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        Result UpdateLog(UpdateLogCriteria criteria);

        Result InsertLog(InsertLogCriteria criteria);

        BaseCommunicationLog GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria);

        CustomerDefaultSetting GetCustomerDefaultSetting(GetDefaultSettingsCriteria criteria);
    }
}
