namespace Fanex.CRM.Feedback.Services.AddCustomerFeedback
{
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Repositories.AddCustomerFeedback;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;

    public class AddGuestFeedbackService : IAddGuestFeedbackService
    {
        private readonly IAddGuestFeedbackRepository _addGuestFeedbackRepository;

        public AddGuestFeedbackService()
            : this(null)
        {
        }

        public AddGuestFeedbackService(
            IAddGuestFeedbackRepository addGuestFeedbackRepository)
        {
            _addGuestFeedbackRepository = addGuestFeedbackRepository ?? new AddGuestFeedbackRepository();
        }

        public Result AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            var result = _addGuestFeedbackRepository.AddGuestFeedback(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Feedback_AddCustomerFeedback_AddSuccess;
            }

            return result;
        }
    }
}
