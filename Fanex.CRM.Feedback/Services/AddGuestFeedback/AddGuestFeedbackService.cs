namespace Fanex.CRM.Feedback.Services.AddCustomerFeedback
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Infrastructure.Constants;

    public class AddGuestFeedbackService : IAddGuestFeedbackService
    {
        private readonly IBaseService baseService;

        public AddGuestFeedbackService() : this(null)
        {
        }

        public AddGuestFeedbackService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public CommandResult AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Feedback_AddCustomerFeedback_AddSuccess;
            }

            return result;
        }
    }
}