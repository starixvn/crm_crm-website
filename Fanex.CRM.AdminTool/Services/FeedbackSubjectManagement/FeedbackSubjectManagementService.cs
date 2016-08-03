// --------------------------------------------------------------------------
// <copyright file="FeedbackSubjectManagementService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.FeedbackSubjectManagement
{
    using System.Linq;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Repositories.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Resources;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Services;

    /// <summary>
    /// Feedback Subject Management Service.
    /// </summary>
    public class FeedbackSubjectManagementService : IFeedbackSubjectManagementService
    {
        private IFeedbackSubjectManagementRepository _feedbackSubjectManagementRepository;

        public FeedbackSubjectManagementService()
            : this(null)
        {
        }

        public FeedbackSubjectManagementService(IFeedbackSubjectManagementRepository feedbackSubjectManagementRepository)
        {
            _feedbackSubjectManagementRepository = feedbackSubjectManagementRepository ?? new FeedbackSubjectManagementRepository();
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

        public Result ChangeTemplateState(ChangeSubjectTemplateStateCriteria criteria)
        {
            var result = _feedbackSubjectManagementRepository.ChangeTemplateState(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = criteria.IsActive ?
                    Labels.ActiveFeedbackSubjectSuccessMessage :
                    Labels.DeactiveFeedbackSubjectSuccessMessage;

                CacheService.Delete(SettingService.GetAllFeedbackTemplateSubjectSessionKey);
            }

            return result;
        }

        public Result EditTemplate(EditFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementRepository.EditTemplate(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Feedback_FeedbackSubjectManagement_EditSuccess;
                CacheService.Delete(SettingService.GetAllFeedbackTemplateSubjectSessionKey);
            }

            return result;
        }

        public Result InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementRepository.InsertTemplate(criteria);
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
