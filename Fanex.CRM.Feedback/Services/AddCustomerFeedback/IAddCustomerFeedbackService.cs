namespace Fanex.CRM.Feedback.Services.AddCustomerFeedback
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;

    public interface IAddCustomerFeedbackService
    {
        CommandResult AddCustomerFeedback(AddCustomerFeedbackCriteria criteria);
    }
}