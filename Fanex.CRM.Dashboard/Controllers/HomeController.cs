// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : ReadOnlyBaseController
    {
        private readonly IPermissionService _permissionService;

        public HomeController()
            : this(null)
        {
        }

        public HomeController(IPermissionService permissionService)
        {
            _permissionService = permissionService ?? new PermissionService();
        }

        public RedirectResult Index()
        {
            return new RedirectResult("~/Home/Default");
        }

        public ActionResult BlankPage()
        {
            return View("~/Views/Home/BlankPage.cshtml");
        }

        /// <summary>
        /// Access default page.
        /// </summary>
        /// <returns>Default page.</returns>
        public ActionResult Default()
        {
            ViewBag.AccUsername = AppSession.CurrentUser.Username;
            ViewBag.CreateCustomerInfoPolicy = _permissionService.GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);

            ViewBag.CurrentCustomer = AppSession.CurrentCustomer;

            return View("~/Views/Home/Default.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult GettingUserMenu()
        {
            return PartialView("~/Views/Home/_Menu.cshtml", AppSession.CurrentUserMenu);
        }

        public PartialViewResult GettingDashboardMenu()
        {
            ViewBag.HasCustomerInfoPermission = HasPermission(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo, Constant.Policy.Read);
            return PartialView("~/Views/Home/_DashboardMenu.cshtml");
        }

        public PartialViewResult SearchCustomerRemind()
        {
            return PartialView("~/Views/Home/_SearchCustomerRemind.cshtml");
        }
    }
}