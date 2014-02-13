// --------------------------------------------------------------------------
// <copyright file="DashboardController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Services.Implements;
    using Fanex.CRM.Dashboard.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Web.Controllers;

    /// <summary>
    /// Dashboard Controller.
    /// </summary>
    public class DashboardController : BaseController
    {
        private IDashboardService _serviceInstance;

        public DashboardController()
            : this(null)
        {
        }

        public DashboardController(IDashboardService serviceInstance)
        {
            _serviceInstance = serviceInstance ?? new DashboardService();
        }

        public ActionResult Index(int customerId)
        {
            return View("~/Areas/Dashboard/Index.cshtml", customerId);
        }

        [HttpPost]
        public ActionResult GetCustomerDashboardInfo(GetCustomerDashboardInfoCriteria criteria)
        {
            var result = _serviceInstance.GetCustomerDashboardInfo(criteria);
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
