namespace Fanex.CRM.Feedback.Services.AddCustomerFeedback
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public interface IAddCustomerFeedbackService
    {
        Result AddCustomerFeedback(AddCustomerFeedbackCriteria criteria);
    }
}
