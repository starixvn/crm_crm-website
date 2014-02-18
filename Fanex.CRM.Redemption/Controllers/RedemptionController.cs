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
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Services.Implements;
    using Fanex.CRM.Redemption.Services.Interfaces;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Redemption Controller.
    /// </summary>
    public class RedemptionController : BaseController
    {
        private IRedemptionLogService _serviceInstance;

        public RedemptionController()
            : this(null)
        {
        }

        public RedemptionController(IRedemptionLogService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new RedemptionLogService();
        }

        [NapAuth(FunctionKey = FunctionPermission.RedemptionManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index(int customerId)
        {
            return View("~/Areas/Redemption/ViewLog/Index.cshtml", customerId);
        }

        [NapAuth(FunctionKey = FunctionPermission.RedemptionManagement, Policy = Constant.Policy.Host)]
        public ActionResult OpenEditView(int logId)
        {
            var logInfo = _serviceInstance.GetLogById(new GetLogByIdCriteria
            {
                Id = logId
            });

            return View("~/Areas/Redemption/EditLog/Index.cshtml", logInfo);
        }

        [NapAuth(FunctionKey = FunctionPermission.RedemptionManagement, Policy = Constant.Policy.Host)]
        public ActionResult OpenInsertView(int customerId)
        {
            return View("~/Areas/Redemption/InsertLog/Index.cshtml", customerId);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.RedemptionManagement, Policy = Constant.Policy.Host)]
        public ActionResult GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            var result = _serviceInstance.GetLogsByCustomerId(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.RedemptionManagement, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLog(UpdateLogCriteria criteria)
        {
            var result = _serviceInstance.UpdateLog(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.RedemptionManagement, Policy = Constant.Policy.Host)]
        public JsonResult InsertLog(InsertLogCriteria criteria)
        {
            var result = _serviceInstance.InsertLog(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAllStatus()
        {
            var result = _serviceInstance.GetAllStatus();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
