namespace Fanex.CRM.Feedback.Services.AddCustomerFeedback
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Repositories.AddCustomerFeedback;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;

    public class AddCustomerFeedbackService : IAddCustomerFeedbackService
    {
        private readonly IAddCustomerFeedbackRepository _addCustomerFeedbackRepository;

        public AddCustomerFeedbackService()
            : this(null)
        {
        }

        public AddCustomerFeedbackService(
            IAddCustomerFeedbackRepository addCustomerFeedbackRepository)
        {
            _addCustomerFeedbackRepository = addCustomerFeedbackRepository ?? new AddCustomerFeedbackRepository();
        }

        public Result AddCustomerFeedback(AddCustomerFeedbackCriteria criteria)
        {
            var result = _addCustomerFeedbackRepository.AddCustomerFeedback(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Feedback_AddCustomerFeedback_AddSuccess;
            }

            return result;
        }
    }
}
