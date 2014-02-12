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
        public ActionResult Index(GetRemarkCriteria criteria)
        {
#if DEBUG
            criteria.CustId = 398615;
#endif
            var remarks = _serviceInstance.GetRemarks(criteria);
            return View("~/Areas/Remark/Index.cshtml", remarks);
        }

        [NapAuth(FunctionKey = FunctionPermission.CustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult Update(UpdateRemarkCriteria criteria)
        {
            var result = _serviceInstance.UpdateRemarks(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
