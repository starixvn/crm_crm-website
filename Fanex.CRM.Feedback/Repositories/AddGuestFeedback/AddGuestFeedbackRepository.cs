namespace Fanex.CRM.Feedback.Repositories.AddCustomerFeedback
{
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public class AddGuestFeedbackRepository : BaseRepository, IAddGuestFeedbackRepository
    {
        public Result AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
