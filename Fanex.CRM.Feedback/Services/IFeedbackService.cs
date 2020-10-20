namespace Fanex.CRM.Feedback.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;

    public interface IFeedbackService
    {
        IEnumerable<BaseModel> GetTopItems { get; }

        dynamic GetAllSettings();

        IEnumerable<DeletedFeedback> GetManageFeedbacks(ManageFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetLatestFeedbackReport(LatestFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetDeletedFeedbackReport(DeletedFeedbackCriteria criteria);

        FeedbackDuplicationResult CheckFeedbackDuplication(CheckFeedbackDuplicationCriteria criteria);
    }
}