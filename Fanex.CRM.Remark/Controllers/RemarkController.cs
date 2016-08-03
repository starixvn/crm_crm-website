// --------------------------------------------------------------------------
// <copyright file="RemarkController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Remark.Criteria;
    using Fanex.CRM.Remark.Models;
    using Fanex.CRM.Remark.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Remark controller.
    /// </summary>
    [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Read)]
    public class RemarkController : ReadOnlyBaseController
    {
        private readonly IRemarkService _remarkService;

        public RemarkController()
            : this(null)
        {
        }

        public RemarkController(IRemarkService remarkService)
        {
            _remarkService = remarkService ?? new RemarkService();
        }
        
        [HttpGet]
        public ActionResult Index(int customerId)
        {
            ViewBag.CustomerInfoPolicy = GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);

            return View("~/Areas/Remark/Index.cshtml", customerId);
        }

        [HttpGet]
        public ActionResult GetRemarks(GetRemarkCriteria criteria)
        {
            var remarks = _remarkService.GetRemarks(criteria);
            remarks.CustId = criteria.CustomerId;

            return Json(remarks, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult Update(Remarks model)
        {
            var result = _remarkService.UpdateRemarks(model);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
