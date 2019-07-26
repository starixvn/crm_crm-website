namespace Fanex.CRM.Communication.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface ICommunicationLogService
    {
        IList<BaseModel> AllStatusForEdit { get; }

        IList<BaseModel> AllStatusForInsert { get; }

        CustomerLogInfo GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        BaseCommunicationLog GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria);

        CommandResult UpdateLog(UpdateLogCriteria criteria);

        CommandResult InsertLog(InsertLogCriteria criteria);

        CustomerDefaultSetting GetCustomerDefaultSetting(GetDefaultSettingsCriteria criteria);
    }
}