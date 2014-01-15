// --------------------------------------------------------------------------
// <copyright file="AddFeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Impl
{
    using System;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Interface;

    /// <summary>
    /// Feedback service.
    /// </summary>
    public partial class FeedbackService : IFeedbackService
    {
        #region add guest feedback

        /// <summary>
        /// Adding guest feedback.
        /// </summary>
        /// <param name="feedback">Feedback info.</param>
        /// <returns>Return result.</returns>
        public Result AddGuestFeedback(Feedback feedback)
        {
            var ip = Helper.GetClientIP();
            var entity = feedback;
            Result result = new Result();
            try
            {
                Check.Argument.IsValidIP(ip, "Ip");
                result = CheckFeedbackPermission(feedback.Status.Id);
                if (result != null && result.ErrCode != 0)
                {
                    return result;
                }

                _repository.AddGuestFeedback(entity, ip, ref result);
            }
            catch (Exception ex)
            {
                result = new Result(-100, ex.Message);
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        #endregion

        #region add new feedback

        /// <summary>
        /// Adding new feedback.
        /// </summary>
        /// <param name="feedback">Feedback info.</param>
        /// <returns>Return result.</returns>
        public Result AddFeedback(Feedback feedback)
        {
            var entity = feedback;
            var ip = Helper.GetClientIP();
            Result result = new Result();
            try
            {
                Check.Argument.IsValidIP(ip, "Ip");
                result = CheckFeedbackPermission(feedback.Status.Id);
                if (result != null && result.ErrCode != 0)
                {
                    return result;
                }

                _repository.AddFeedback(entity, ip, ref result);
            }
            catch (Exception ex)
            {
                result = new Result(100, ex.Message);
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Add Subject Feedback Template.
        /// </summary>
        /// <param name="subject">Subject Feedback Template.</param>
        /// <returns>Return result.</returns>
        public Result AddSubjectFeedbackTemplate(string subject)
        {
            Result result = new Result(0, string.Empty);
            try
            {
                if (!_permissionService.GetHostPermission(Enumerations.Functions.AdminTool.FBSubjectTemplateManagement))
                {
                    return ReturnErrorResult(500, "lblNoInsertFBSubjectTemplate");
                }

                result = _repository.AddSubjectFeedbackTemplate(subject);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}