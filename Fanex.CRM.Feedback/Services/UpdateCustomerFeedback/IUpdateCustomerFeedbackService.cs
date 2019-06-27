namespace Fanex.CRM.Feedback.Services
{
    using Common.Enumerations;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;

    public interface IUpdateCustomerFeedbackService
    {
        dynamic GetAllSettings(bool hasDeleteFeedbackPermission, UserRole userRole);

        CustomerFeedback GetFeedbackById(GetFeedbackByIdCriteria criteria);

        void UpdateFeedback(UpdateCustomerFeedbackCriteria criteria);
    }
}
