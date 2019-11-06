namespace Fanex.CRM.Feedback.Services
{
    using System.Collections.Generic;
    using Common.Models;
    using Criteria;
    using Fanex.CRM.Feedback.Models;

    public interface IMultipleLogsService
    {
        IList<BaseModel> AllStatusForInsert { get; }

        IList<BaseModel> AllStatusForEdit { get; }

        IEnumerable<VerifyCustomer> CheckNotExistCommunicationToolSetting(CheckNotExistCommunicationToolSettingCriteria criteria);

        CommandResult InsertMultpleLogs(InsertMultipleLogsCriteria criteria);

        CommandResult UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria);

        BaseCommunicationLog GetLogByFeedbackId(GetLogByFeedbackIdCriteria criteria);

        CommandResult UpdateMultipleLogStatuses(UpdateMultipleLogStatusesCriteria criteria);
    }
}