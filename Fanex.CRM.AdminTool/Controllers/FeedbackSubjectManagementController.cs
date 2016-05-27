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
    using Fanex.CRM.Common.Attributes;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// Feedback Subject Management Controller.
    /// </summary>
    public class FeedbackSubjectManagementController : ReadOnlyBaseController
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
        public ActionResult OpenChangeTemplateStateView(ChangeSubjectTemplateStateCriteria template)
        {
            return View("~/Areas/AdminTool/FeedbackSubjectManagement/ChangeTemplateState/Index.cshtml", template);
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
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.FBSubjectTemplateManagement, Policy = Constant.Policy.Host)]
        public JsonResult ChangeTemplateState(ChangeSubjectTemplateStateCriteria criteria)
        {
            Result result = _feedbackSubjectManagementService.ChangeTemplateState(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthenticateAction(AuthenticationType.Read, "Index")]
        public JsonResult FeedbackSubjectTemplateById(FeedbackSubjectTemplateByIdCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.GetFeedbackSubjectTemplate(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.FBSubjectTemplateManagement, Policy = Constant.Policy.Host)]
        public JsonResult EditTemplate(EditFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.EditTemplate(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.FBSubjectTemplateManagement, Policy = Constant.Policy.Host)]
        public JsonResult InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.InsertTemplate(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
