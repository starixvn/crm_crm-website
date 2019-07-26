namespace Fanex.CRM.Feedback.Services.AddCustomerFeedback
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Infrastructure.Constants;

    public class AddCustomerFeedbackService : IAddCustomerFeedbackService
    {
        private readonly IBaseService baseService;

        public AddCustomerFeedbackService() : this(null)
        {
        }

        public AddCustomerFeedbackService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public CommandResult AddCustomerFeedback(AddCustomerFeedbackCriteria criteria)
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