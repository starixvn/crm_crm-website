using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Common.Models;
using Fanex.CRM.Feedback.Criteria;
using Fanex.CRM.Feedback.Services;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Fanex.CRM.Feedback.Controllers
{
    public class QuickUpdateFeedbackController : ReadOnlyBaseController
    {
        private readonly IQuickUpdateFeedbackService _quickUpdateFeedbackService;

        public QuickUpdateFeedbackController()
            : this(null)
        {
        }

        public QuickUpdateFeedbackController(IQuickUpdateFeedbackService quickUpdateFeedbackService)
        {
            _quickUpdateFeedbackService = quickUpdateFeedbackService ?? new QuickUpdateFeedbackService();
        }

        public ActionResult Index(int feedbackId)
        {
            ViewBag.feedbackId = feedbackId;
            return View("~/Areas/Feedbacks/QuickUpdate/Index.cshtml");
        }

        public ActionResult GetFeedback(GetQuickUpdateFeedbackCriteria criteria)
        {
            return Json(_quickUpdateFeedbackService.GetFeedback(criteria), JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult UpdateFeedback(UpdateQuickUpdateFeedbackCriteria criteria)
        {
            _quickUpdateFeedbackService.UpdateFeedback(criteria);
            var result = new CommandResult(criteria.ErrorCode, criteria.ErrorMessage);          

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
