namespace Fanex.CRM.Feedback.Repositories
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data.Repository;
    using Infrastructure.Models;
    using System.Collections.Generic;

    public interface IFeedbackRepository
    {
        IEnumerable<FeedbackDetail> GetManagerFeedbacks(ManageFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetDeletedFeedbacks(ICriteria criteria);

        Result UpdateSiteLicenseeTypes(UpdateSiteLicenseeTypesCriteria criteria);

        int CheckFeedbackDuplication(CheckFeedbackDuplicationCriteria criteria);
    }
}
