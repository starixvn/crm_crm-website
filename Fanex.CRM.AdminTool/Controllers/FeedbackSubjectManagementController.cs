// --------------------------------------------------------------------------
// <copyright file="FeedbackSubjectManagementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Services.FeedbackSubjectManagement;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Feedback Subject Management Controller.
    /// </summary>
    public class FeedbackSubjectManagementController : BaseController
    {
        private IFeedbackSubjectManagementService _feedbackSubjectManagementService;

        public FeedbackSubjectManagementController()
            : this(null)
        {
        }

        public FeedbackSubjectManagementController(IFeedbackSubjectManagementService feedbackSubjectManagementService)
        {
            _feedbackSubjectManagementService = feedbackSubjectManagementService ?? new FeedbackSubjectManagementService();
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Index.cshtml");
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult OpenDeleteTemplateView(int templateId)
        {
            return View("~/Areas/AdminTool/FeedbackSubjectManagement/DeleteTemplate/Index.cshtml", templateId);
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult OpenEditTemplateView(int templateId)
        {
            return View("~/Areas/AdminTool/FeedbackSubjectManagement/EditTemplate/Index.cshtml", templateId);
        }

        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public ActionResult OpenInsertTemplateView()
        {
            return View("~/Areas/AdminTool/FeedbackSubjectManagement/InsertTemplate/Index.cshtml");
        }

        /// <summary>
        /// Adds the feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Add result.
        /// </returns>
        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.SearchFeedbackSubjectTemplate(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Host, "Index")]
        public JsonResult DeleteTemplate(DeleteFeedbackSubjectTemplateCriteria criteria)
        {
            Result result = _feedbackSubjectManagementService.DeleteTemplate(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult FeedbackSubjectTemplateById(FeedbackSubjectTemplateByIdCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.FeedbackSubjectTemplateById(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Host, "Index")]
        public JsonResult EditTemplate(EditFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.EditTemplate(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Host, "Index")]
        public JsonResult InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.InsertTemplate(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
