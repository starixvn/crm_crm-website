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

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : BaseController
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
            if (AppSession.CurrentUserMenu == null || AppSession.CurrentUser == null)
            {
                return new RedirectResult("~/UserLogin/SignIn", true);
            }
            else
            {
                return new RedirectResult("~/Home/Default");
            }
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
            if (AppSession.CurrentUserMenu == null || AppSession.CurrentUser == null)
            {
                return new RedirectResult("~/UserLogin/SignIn");
            }

            ViewBag.RedirectUrl = string.Empty;
            ViewBag.AccUsername = AppSession.CurrentUser.Username;
            ViewBag.CreateCustomerInfoPolicy = _permissionService.GetPolicy(FunctionPermissions.CreateUpdateFeedbackAndCustomerInfo);

            if (AppSession.RedirectLink != null && 
                !string.IsNullOrEmpty(AppSession.RedirectLink))
            {
                ViewBag.RedirectUrl = AppSession.RedirectLink;
                AppSession.RedirectLink = null;
            }

            return View("~/Views/Home/Default.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult GettingUserMenu()
        {
            ViewBag.MenuItemList = CurrentSession.MenuItemList;
            return PartialView("~/Views/Home/_Menu.cshtml");
        }

        public PartialViewResult GettingDashboardMenu()
        {
            return PartialView("~/Views/Home/_DashboardMenu.cshtml");
        }

        public PartialViewResult SearchCustomerRemind()
        {
            return PartialView("~/Views/Home/_SearchCustomerRemind.cshtml");
        }
    }
}