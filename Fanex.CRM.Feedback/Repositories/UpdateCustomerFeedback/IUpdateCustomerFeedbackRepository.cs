namespace Fanex.CRM.Feedback.Repositories
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.Data.Repository;

    public interface IUpdateCustomerFeedbackRepository
    {
        CustomerFeedback GetFeedbackById(ICriteria criteria);

        void UpdateFeedback(UpdateCustomerFeedbackCriteria criteria);
    }
}
