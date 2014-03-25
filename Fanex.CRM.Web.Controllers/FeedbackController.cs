// --------------------------------------------------------------------------
// <copyright file="FeedbackController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Feedback Controller.
    /// </summary>
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        private IPermissionService _permission;

        public FeedbackController()
            : this(null, null)
        {
        }

        public FeedbackController(IFeedbackService feedbackService, IPermissionService permissionService)
        {
            _feedbackService = feedbackService ?? new FeedbackService();
            _permission = permissionService ?? new PermissionService();
        }

        #region Views
        [NapAuth(FunctionKey = FunctionPermission.CreateFeedback, Policy = Constant.Policy.Read)]
        public ActionResult AddFeedback(int custId, string custName)
        {
            ViewBag.Title = "Add Feedback";
            return View();
        }

        [NapAuth(FunctionKey = FunctionPermission.CreateFeedback, Policy = Constant.Policy.Read)]
        public ActionResult AddGuestFeedback()
        {
            ViewBag.Title = "Add Guest Feedback";
            return View();
        }

        /// <summary>
        /// Deletes the feedback.
        /// </summary>
        /// <returns>Delete result.</returns>
        [NapAuth(FunctionKey = FunctionPermission.DeleteFeedback, Policy = Constant.Policy.Host)]
        public ActionResult DeletedFeedback()
        {
            return View();
        }

        public ActionResult LastestFeedback()
        {
            return View();
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult ManageFeedback()
        {
            return View();
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult CustomerLastestFeedback()
        {
            return View();
        }

        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult EditFeedback()
        {
            return View();
        }

        #endregion

        #region Post Actions

        /// <summary>
        /// Adds the guest feedback.
        /// </summary>
        /// <param name="feedback">The feedback.</param>
        /// <returns>Add result.</returns>
        [HttpPost]
        public JsonResult AddGuestFeedback(Feedback feedback)
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.AddGuestFeedback(feedback);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Selects the top feedback by customer identifier.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="selectTop">The select top.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="fromCache">If set to <c>true</c> [from cache].</param>
        /// <returns>The top feedback by customer identifier.</returns>
        [HttpPost]
        public JsonResult SelTopFeedbackByCustId(int custId, int selectTop, int pageIndex, int pageSize, bool fromCache)
        {
            var result = new DataGridViewDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.SelTopFeedbackByCustId(custId, selectTop, pageIndex, pageSize, fromCache);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                pageindex = result.PageIndex,
                total = result.TotalItems,
                rows = result.Rows
            });

            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the deleted feedback.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="isGuest">The is guest.</param>
        /// <param name="communicationID">The communication identifier.</param>
        /// <param name="mainCategoryID">The main category identifier.</param>
        /// <param name="subCategoryID">The sub category identifier.</param>
        /// <param name="isReviewed">The is reviewed.</param>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="fromCache">If set to <c>true</c> [from cache].</param>
        /// <returns>The deleted feedback.</returns>
        [HttpPost]
        public JsonResult GetDeletedFeedback(
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
            var result = new DataGridViewDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetDeletedFeedback(
                    fromDate,
                    toDate,
                    userName,
                    isGuest,
                    communicationID,
                    mainCategoryID,
                    subCategoryID,
                    isReviewed,
                    responseId,
                    pageIndex,
                    pageSize,
                    fromCache);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                pageindex = result.PageIndex,
                total = result.TotalItems,
                rows = result.Rows
            });
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets the feedback.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="username">The username.</param>
        /// <param name="isGuest">The is guest.</param>
        /// <param name="communicationId">The communication identifier.</param>
        /// <param name="mainCategoryId">The main category identifier.</param>
        /// <param name="subCategoryId">The sub category identifier.</param>
        /// <param name="statusId">The status identifier.</param>
        /// <param name="isReviewed">The is reviewed.</param>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="fromCache">If set to <c>true</c> [from cache].</param>
        /// <returns>The feedback.</returns>
        [HttpPost]
        public JsonResult GetFeedback(
            string fromDate,
            string toDate,
            int? runningNumber,
            string username,
            int isGuest,
            int communicationId,
            int mainCategoryId,
            int subCategoryId,
            int statusId,
            int isReviewed,
            int responseId,
            int pageIndex,
            int pageSize,
            bool fromCache)
        {
            var result = new DataGridViewDTO();
            JsonResult jsonResult;
            try
            {
                int feedbackId = runningNumber.HasValue ? runningNumber.Value : 0;
                result = _feedbackService.GetFeedback(
                    fromDate,
                    toDate,
                    feedbackId,
                    username,
                    isGuest,
                    communicationId,
                    mainCategoryId,
                    subCategoryId,
                    statusId,
                    isReviewed,
                    responseId,
                    pageIndex,
                    pageSize,
                    fromCache);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                pageindex = result.PageIndex,
                total = result.TotalItems,
                rows = result.Rows
            });
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Selects the top feedback.
        /// </summary>
        /// <param name="selectTop">The select top.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="fromCache">If set to <c>true</c> [from cache].</param>
        /// <returns>The top feedback.</returns>
        [HttpPost]
        public JsonResult SelTopFeedback(int selectTop, int pageIndex, int pageSize, bool fromCache)
        {
            var result = new DataGridViewDTO();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.SelTopFeedback(selectTop, pageIndex, pageSize, fromCache);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(new
            {
                pageindex = result.PageIndex,
                total = result.TotalItems,
                rows = result.Rows
            });
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Gets by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get result.</returns>
        [HttpPost]
        public JsonResult GetById(int id)
        {
            var result = new Feedback();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.GetById(id);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Edits the feedback.
        /// </summary>
        /// <param name="feedback">The feedback.</param>
        /// <returns>Edit result.</returns>
        [HttpPost]
        public JsonResult EditFeedback(Feedback feedback)
        {
            var result = new Result();
            JsonResult jsonResult;
            try
            {
                result = _feedbackService.EditFeedback(feedback);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        /// <summary>
        /// Adds the feedback.
        /// </summary>
        /// <param name="feedback">The feedback.</param>
        /// <returns>Add result.</returns>
        [HttpPost]
        public JsonResult AddFeedback(Feedback feedback)
        {
            var result = new Result();
            try
            {
                var userId = AppSession.CurrentUser.Id;
                feedback.Creator = new User
                {
                    Id = userId
                };

                if (feedback.IsReviewed)
                {
                    feedback.Reviewer = new User
                    {
                        Id = userId
                    };
                }

                result = _feedbackService.AddFeedback(feedback);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        #endregion
    }
}