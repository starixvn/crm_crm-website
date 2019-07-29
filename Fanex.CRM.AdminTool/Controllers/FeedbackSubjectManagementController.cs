namespace Fanex.CRM.AdminTool.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Services.FeedbackSubjectManagement;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Models;
    using Fanex.NAP.Core.Business;
    using NAP.Core.Web.Attribute;

    [NapAuth(FunctionKey = FunctionPermissions.FBSubjectTemplateManagement, Policy = Constant.Policy.Read)]
    public class FeedbackSubjectManagementController : ReadOnlyBaseController
    {
        private readonly IFeedbackSubjectManagementService _feedbackSubjectManagementService;

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
            ViewBag.FeedbackSubjectManagementPolicy = GetPolicy(FunctionPermissions.FBSubjectTemplateManagement);
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
            CommandResult result = _feedbackSubjectManagementService.ChangeTemplateState(criteria);
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