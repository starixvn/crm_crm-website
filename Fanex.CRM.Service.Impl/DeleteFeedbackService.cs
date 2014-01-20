// --------------------------------------------------------------------------
// <copyright file="DeleteFeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using DNAide.Core;
    using Fanex.CRM.DataAccess.Impl;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Interface;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// Feedback service.
    /// </summary>
    public partial class FeedbackService : IFeedbackService
    {
        #region get deleted feedback

        /// <summary>
        /// Get Deleted Feedback.
        /// </summary>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="isGuest">Is Guest value.</param>
        /// <param name="communicationID">Communication ID.</param>
        /// <param name="mainCategoryID">MainCategory ID.</param>
        /// <param name="subCategoryID">SubCategory ID.</param>
        /// <param name="isReviewed">Is Reviewed.</param>
        /// <param name="responseId">Response Id.</param>
        /// <param name="pageIndex">Page Index.</param>
        /// <param name="pageSize">Page Size.</param>
        /// <param name="fromCache">From Cache.</param>
        /// <returns>Return result.</returns>
        public DataGridViewDTO GetDeletedFeedback(
            string fromDate,
            string toDate,
            string userName,
            int isGuest,
            int communicationID,
            int mainCategoryID,
            int subCategoryID,
            int isReviewed,
            int responseId,
            int pageIndex,
            int pageSize,
            bool fromCache)
        {
            var feedbackdto = new DataGridViewDTO();
            try
            {
                List<FeedbackView> lstFeedback = null;
                if (fromCache)
                {
                    lstFeedback = (List<FeedbackView>)SessionExtension.GetSession(HttpContext.Current.Session, _deletedFeedback) ??
                        SelAllDeletedFeedback(fromDate, toDate, userName, isGuest, communicationID, mainCategoryID, subCategoryID, isReviewed, responseId);
                }
                else
                {
                    lstFeedback = SelAllDeletedFeedback(fromDate, toDate, userName, isGuest, communicationID, mainCategoryID, subCategoryID, isReviewed, responseId);
                }

                var lstFeedbackPaging = new List<FeedbackView>();
                if (lstFeedback != null && lstFeedback.Count() > 0)
                {
                    lstFeedbackPaging = lstFeedback.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    Parallel.ForEach(
                        lstFeedbackPaging,
                        feedback =>
                        {
                            feedback.MainCategory = _settingService.GetById(feedback.MainCategoryId).Value;
                            feedback.SubCategory = _settingService.GetById(feedback.SubCategoryId).Value;
                            feedback.Status = _settingService.GetById(feedback.StatusId).Value;
                        });
                }

                feedbackdto.Rows = lstFeedbackPaging;
                feedbackdto.TotalItems = lstFeedback != null ? lstFeedback.Count() : 0;
                feedbackdto.PageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return feedbackdto;
        }
        #endregion

        /// <summary>
        /// Delete Subject Feedback Template.
        /// </summary>
        /// <param name="id">Subject Feedback Template Id.</param>
        /// <returns>Return result.</returns>
        public Result DeleteSubjectFeedbackTemplate(int id)
        {
            Result result = new Result(0, string.Empty);
            try
            {
                if (!_permissionService.HasPermission(FunctionPermission.FBSubjectTemplateManagement, Constant.Policy.Host))
                {
                    return ReturnErrorResult(500, "lblNoDeleteFBSubjectTemplate");
                }

                result = _repository.DeleteSubjectFeedbackTemplate(id);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Select All Deleted Feedback.
        /// </summary>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="userName">User Name.</param>
        /// <param name="isGuest">Is Guest value.</param>
        /// <param name="communicationID">Communication ID.</param>
        /// <param name="mainCategoryID">MainCategory ID.</param>
        /// <param name="subCategoryID">SubCategory ID.</param>
        /// <param name="isReviewed">Is Reviewed.</param>
        /// <param name="responseId">Response Id.</param>
        /// <returns>Return result.</returns>
        private List<FeedbackView> SelAllDeletedFeedback(
            string fromDate,
            string toDate,
            string userName,
            int isGuest,
            int communicationID,
            int mainCategoryID,
            int subCategoryID,
            int isReviewed,
            int responseId)
        {
            var lstFeedback = _repository.GetDeletedFeedback(
                CastAide.AsDateTime(fromDate),
                CastAide.AsDateTime(toDate),
                userName,
                isGuest,
                communicationID,
                mainCategoryID,
                subCategoryID,
                isReviewed,
                responseId).ToList();
            var result = new List<FeedbackView>();
            if (lstFeedback != null && lstFeedback.Count > 0)
            {
                foreach (Feedback feedback in lstFeedback)
                {
                    result.Add(CreateFeedbackView(feedback));
                }
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _deletedFeedback, result);
            return result;
        }
    }
}