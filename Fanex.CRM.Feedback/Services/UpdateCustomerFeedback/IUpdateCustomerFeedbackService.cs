namespace Fanex.CRM.Feedback.Services
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;

    public interface IUpdateCustomerFeedbackService
    {
        dynamic GetAllSettings(bool hasDeleteFeedbackPermission);

        CustomerFeedback GetFeedbackById(GetFeedbackByIdCriteria criteria);

        void UpdateFeedback(UpdateCustomerFeedbackCriteria criteria);
    }
}
