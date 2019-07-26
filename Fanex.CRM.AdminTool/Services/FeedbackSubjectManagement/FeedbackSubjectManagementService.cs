namespace Fanex.CRM.AdminTool.Services.FeedbackSubjectManagement
{
    using System.Linq;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Repositories.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Resources;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Setting.Services;

    public class FeedbackSubjectManagementService : IFeedbackSubjectManagementService
    {
        private readonly IFeedbackSubjectManagementRepository _feedbackSubjectManagementRepository;
        private readonly IBaseService baseService;

        public FeedbackSubjectManagementService() : this(null, null)
        {
        }

        public FeedbackSubjectManagementService(
            IFeedbackSubjectManagementRepository feedbackSubjectManagementRepository,
            IBaseService baseService)
        {
            _feedbackSubjectManagementRepository = feedbackSubjectManagementRepository ?? new FeedbackSubjectManagementRepository();
            this.baseService = baseService ?? new BaseService();
        }

        public FeedbackSubjectTemplateView SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria)
        {
            var repositoryResult = _feedbackSubjectManagementRepository.SearchFeedbackSubjectTemplate(criteria);
            return new FeedbackSubjectTemplateView
            {
                TotalTemplates = repositoryResult.Count(),
                Templates = repositoryResult.Skip(criteria.StartIndex).Take(criteria.PageSize).ToList()
            };
        }

        public CommandResult ChangeTemplateState(ChangeSubjectTemplateStateCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = criteria.IsActive ?
                    Labels.ActiveFeedbackSubjectSuccessMessage :
                    Labels.DeactiveFeedbackSubjectSuccessMessage;

                CacheService.Delete(SettingService.GetAllFeedbackTemplateSubjectSessionKey);
            }

            return result;
        }

        public CommandResult EditTemplate(EditFeedbackSubjectTemplateCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Feedback_FeedbackSubjectManagement_EditSuccess;
                CacheService.Delete(SettingService.GetAllFeedbackTemplateSubjectSessionKey);
            }

            return result;
        }

        public CommandResult InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Feedback_FeedbackSubjectManagement_InsertSuccess;
                CacheService.Delete(SettingService.GetAllFeedbackTemplateSubjectSessionKey);
            }

            return result;
        }

        public FeedbackSubjectTemplate GetFeedbackSubjectTemplate(FeedbackSubjectTemplateByIdCriteria criteria)
        {
            return _feedbackSubjectManagementRepository.GetFeedbackSubjectTemplate(criteria);
        }
    }
}