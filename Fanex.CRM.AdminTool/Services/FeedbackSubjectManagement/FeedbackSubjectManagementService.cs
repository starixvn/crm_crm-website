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
    using Fanex.CRM.Setting.Services.Implements;

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

        /// <summary>
        /// Searches the feedback subject template.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback subject template view.</returns>
        public FeedbackSubjectTemplateView SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria)
        {
            try
            {
                var repositoryResult = _feedbackSubjectManagementRepository.SearchFeedbackSubjectTemplate(criteria);
                return new FeedbackSubjectTemplateView
                {
                    TotalTemplates = repositoryResult.Count,
                    Templates = repositoryResult.Skip(criteria.StartIndex).Take(criteria.PageSize).ToList()
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the template.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Delete result.</returns>
        public Result DeleteTemplate(DeleteFeedbackSubjectTemplateCriteria criteria)
        {
            try
            {
                Result result = _feedbackSubjectManagementRepository.DeleteTemplate(criteria);
                if (result != null && result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = Labels.Feedback_FeedbackSubjectManagement_DeleteSuccess;
                    CacheService.Delete(SettingService.GetAllFeedbackTemplateSubjectSessionKey);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Edits the template.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Edit result.</returns>
        public Result EditTemplate(EditFeedbackSubjectTemplateCriteria criteria)
        {
            try
            {
                Result result = _feedbackSubjectManagementRepository.EditTemplate(criteria);
                if (result != null && result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = Labels.Feedback_FeedbackSubjectManagement_EditSuccess;
                    CacheService.Delete(SettingService.GetAllFeedbackTemplateSubjectSessionKey);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Inserts the template.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria)
        {
            try
            {
                Result result = _feedbackSubjectManagementRepository.InsertTemplate(criteria);
                if (result != null && result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = Labels.Feedback_FeedbackSubjectManagement_InsertSuccess;
                    CacheService.Delete(SettingService.GetAllFeedbackTemplateSubjectSessionKey);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the feedback subject template by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback subject template by identifier.</returns>
        public FeedbackSubjectTemplateInfo FeedbackSubjectTemplateById(FeedbackSubjectTemplateByIdCriteria criteria)
        {
            try
            {
                return _feedbackSubjectManagementRepository.FeedbackSubjectTemplateById(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}
