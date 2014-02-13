// --------------------------------------------------------------------------
// <copyright file="RemarkController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Remark.Criteria;
    using Fanex.CRM.Remark.Models;
    using Fanex.CRM.Remark.Services.Implements;
    using Fanex.CRM.Remark.Services.Interfaces;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Remark controller.
    /// </summary>
    public class RemarkController : BaseController
    {
        private readonly IRemarkService _serviceInstance;

        public RemarkController()
            : this(null)
        {
        }

        public RemarkController(IRemarkService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new RemarkService();
        }

        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult Index(int customerId)
        {
            return View("~/Areas/Remark/Index.cshtml", customerId);
        }

        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult GetRemarks(GetRemarkCriteria criteria)
        {
            var remarks = _serviceInstance.GetRemarks(criteria);
            remarks.CustId = criteria.CustomerId;

            return Json(remarks, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult Update(Remarks model)
        {
            var result = _serviceInstance.UpdateRemarks(model);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
