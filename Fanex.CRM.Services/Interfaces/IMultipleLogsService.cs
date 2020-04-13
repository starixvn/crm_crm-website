namespace Fanex.CRM.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Criteria.MultipleLogs;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.MultipleLogs;

    public interface IMultipleLogsService
    {
        IEnumerable<VerifyCustomerResult> VerifyCustomer(VerifyCustomerCriteria criteria);

        IEnumerable<VerifyRelatedCustomerResult> VerifyRelatedCustomer(VerifyRelatedCustomerCriteria criteria);

        IEnumerable<VerifyCustomer> CheckNotExistCommunicationToolSetting(CheckNotExistCommunicationToolSettingCriteria criteria);

        CommandResult Create(CreateMultipleLogsCriteria criteria);

        CommandResult Update(UpdateMultipleLogsCriteria criteria);

        BaseCommunicationLog GetLogByFeedbackId(GetLogByFeedbackIdCriteria criteria);

        CommandResult UpdateMultipleLogStatus(UpdateMultipleLogStatusCriteria criteria);
    }
}