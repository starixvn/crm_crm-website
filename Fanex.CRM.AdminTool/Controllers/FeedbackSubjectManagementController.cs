// --------------------------------------------------------------------------
// <copyright file="FeedbackSubjectManagementController.cs" company="">
// Copyright (c) . All rights reserved.
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
    using NAP.Core.Web.Attribute;

    /// <summary>
    /// Feedback Subject Management Controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.FBSubjectTemplateManagement, Policy = Constant.Policy.Read)]
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

        public ActionResult Index()
        {
            return View("~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Index.cshtml");
        }

        public ActionResult OpenChangeTemplateStateView(ChangeSubjectTemplateStateCriteria template)
        {
            return View("~/Areas/AdminTool/FeedbackSubjectManagement/ChangeTemplateState/Index.cshtml", template);
        }

        public ActionResult OpenEditTemplateView(int templateId)
        {
            return View("~/Areas/AdminTool/FeedbackSubjectManagement/EditTemplate/Index.cshtml", templateId);
        }

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
        public JsonResult SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.SearchFeedbackSubjectTemplate(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.FBSubjectTemplateManagement, Policy = Constant.Policy.Host)]
        public JsonResult ChangeTemplateState(ChangeSubjectTemplateStateCriteria criteria)
        {
            Result result = _feedbackSubjectManagementService.ChangeTemplateState(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FeedbackSubjectTemplateById(FeedbackSubjectTemplateByIdCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.GetFeedbackSubjectTemplate(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.FBSubjectTemplateManagement, Policy = Constant.Policy.Host)]
        public JsonResult EditTemplate(EditFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.EditTemplate(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.FBSubjectTemplateManagement, Policy = Constant.Policy.Host)]
        public JsonResult InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria)
        {
            var result = _feedbackSubjectManagementService.InsertTemplate(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}