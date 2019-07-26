namespace Fanex.CRM.Redemption.Repositories.RedemptionLog
{
    using System.Collections.Generic;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;

    public interface IRedemptionLogRepository
    {
        IEnumerable<LogInfo> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        LogInfo GetLogById(GetLogByIdCriteria criteria);
    }
}