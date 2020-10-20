namespace Fanex.CRM.Feedback.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data.Repository;

    public interface IFeedbackRepository
    {
        IEnumerable<DeletedFeedback> GetManagerFeedbacks(ManageFeedbackCriteria criteria);

        IEnumerable<DeletedFeedback> GetDeletedFeedbacks(ICriteria criteria);

        int CheckFeedbackDuplication(CheckFeedbackDuplicationCriteria criteria);
    }
}