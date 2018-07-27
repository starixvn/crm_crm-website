namespace Fanex.CRM.Feedback.Repositories.AddCustomerFeedback
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public interface IAddCustomerFeedbackRepository
    {
        Result AddCustomerFeedback(AddCustomerFeedbackCriteria criteria);
    }
}
