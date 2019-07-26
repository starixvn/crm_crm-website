namespace Fanex.CRM.Feedback.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface IFeedbackService
    {
        IEnumerable<BaseModel> GetTopItems { get; }

        dynamic GetAllSettings();

        IEnumerable<FeedbackDetail> GetManageFeedbacks(ManageFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetLatestFeedbackReport(LatestFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetDeletedFeedbackReport(DeletedFeedbackCriteria criteria);

        CommandResult UpdateSiteLicenseeTypes(UpdateSiteLicenseeTypesCriteria criteria);

        FeedbackDuplicationResult CheckFeedbackDuplication(CheckFeedbackDuplicationCriteria criteria);
    }
}