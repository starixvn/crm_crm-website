namespace Fanex.CRM.Dashboard.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;

    public interface IDashboardService
    {
        IEnumerable<string> GetCustomerRemarks(GetCustomerRemarksCriteria criteria);

        IEnumerable<NewRemarkInfo> GetCustomerNewRemarks(GetCustomerNewRemarksCriteria criteria);

        CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria);

        IEnumerable<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria);

        IEnumerable<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria);

        CommandResult UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria);
    }
}