// --------------------------------------------------------------------------
// <copyright file="RedemptionController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Services.RedemptionLog;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using ONEbook.UM.Filters;

    /// <summary>
    /// Redemption Controller.
    /// </summary>
    [CheckUMFilter]
    public class RedemptionController : BaseController
    {
        private IRedemptionLogService _logService;

        public RedemptionController()
            : this(null)
        {
        }

        public RedemptionController(IRedemptionLogService logService)
        {
            _logService = logService ?? new RedemptionLogService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult Index(int customerId)
        {
            ViewBag.CustomerInfoPolicy = GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);
            return View("~/Areas/Redemption/ViewLog/Index.cshtml", customerId);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult OpenEditView(int logId)
        {
            ViewBag.CustomerInfoPolicy = GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);
            ViewBag.HasHostCreateUpdatePermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Host);
            var logInfo = _logService.GetLogById(new GetLogByIdCriteria
            {
                Id = logId
            });

            return View("~/Areas/Redemption/EditLog/Index.cshtml", logInfo);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult OpenInsertView(int customerId)
        {
            return View("~/Areas/Redemption/InsertLog/Index.cshtml", customerId);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            var log = _logService.GetLogsByCustomerId(criteria);
            return Json(log, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLog(UpdateLogCriteria criteria)
        {
            var updateResult = _logService.UpdateLog(criteria);
            return Json(updateResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult InsertLog(InsertLogCriteria criteria)
        {
            var insertResult = _logService.InsertLog(criteria);
            return Json(insertResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public JsonResult AllStatusInsert()
        {
            var settings = _logService.GetAllSettingsInsert();
            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
        public JsonResult AllStatusEdit()
        {
            var allStatus = _logService.GetAllSettingsEdit();
            return Json(allStatus, JsonRequestBehavior.AllowGet);
        }
    }
}
