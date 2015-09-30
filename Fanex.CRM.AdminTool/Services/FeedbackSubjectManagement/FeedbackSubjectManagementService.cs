// --------------------------------------------------------------------------
// <copyright file="FeedbackSubjectManagementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.FeedbackSubjectManagement
{
    using System;
    using System.Linq;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Repositories.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Resources;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Helpers;
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

        public Result DeleteTemplate(DeleteFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementRepository.DeleteTemplate(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.Feedback_FeedbackSubjectManagement_DeleteSuccess;
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
