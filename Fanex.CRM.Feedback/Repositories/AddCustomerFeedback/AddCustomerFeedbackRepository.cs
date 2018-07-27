namespace Fanex.CRM.Feedback.Repositories.AddCustomerFeedback
{
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public class AddCustomerFeedbackRepository : BaseRepository, IAddCustomerFeedbackRepository
    {
        public Result AddCustomerFeedback(AddCustomerFeedbackCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
