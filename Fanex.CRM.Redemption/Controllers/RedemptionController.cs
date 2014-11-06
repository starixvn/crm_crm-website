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
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Services.RedemptionLog;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Redemption Controller.
    /// </summary>
    public class RedemptionController : BaseController
    {
        private IRedemptionLogService _logServiceInstance;

        public RedemptionController()
            : this(null)
        {
        }

        public RedemptionController(IRedemptionLogService logServiceInstance)
        {
            _logServiceInstance = logServiceInstance ?? new RedemptionLogService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult Index(int customerId)
        {
            return View("~/Areas/Redemption/ViewLog/Index.cshtml", customerId);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult OpenEditView(int logId)
        {
            var logInfo = _logServiceInstance.GetLogById(new GetLogByIdCriteria
            {
                Id = logId
            });

            return View("~/Areas/Redemption/EditLog/Index.cshtml", logInfo);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CustomerInfo, Policy = Constant.Policy.Host)]
        public ActionResult OpenInsertView(int customerId)
        {
            return View("~/Areas/Redemption/InsertLog/Index.cshtml", customerId);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CustomerInfo, Policy = Constant.Policy.Read)]
        public ActionResult GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            var log = _logServiceInstance.GetLogsByCustomerId(criteria);
            var jsonResult = Json(log, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLog(UpdateLogCriteria criteria)
        {
            var updateResult = _logServiceInstance.UpdateLog(criteria);
            var jsonResult = Json(updateResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.CustomerInfo, Policy = Constant.Policy.Host)]
        public JsonResult InsertLog(InsertLogCriteria criteria)
        {
            var insertResult = _logServiceInstance.InsertLog(criteria);
            var jsonResult = Json(insertResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllStatusInsert()
        {
            var allStatus = _logServiceInstance.AllStatusInsert();
            var jsonResult = Json(allStatus, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AllStatusEdit()
        {
            var allStatus = _logServiceInstance.AllStatusEdit();
            var jsonResult = Json(allStatus, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
