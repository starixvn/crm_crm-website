namespace Fanex.CRM.Dashboard.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;

    public interface IDashboardRepository
    {
        CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria);

        IEnumerable<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria);

        IEnumerable<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria);
    }
}