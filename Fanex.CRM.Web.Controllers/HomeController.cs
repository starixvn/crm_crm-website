// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;

    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : BaseController
    {
        private IPermissionService _permission;

        public HomeController()
            : this(null)
        {
        }

        public HomeController(IPermissionService permission)
        {
            _permission = permission ?? new PermissionService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BlankPage()
        {
            return View();
        }

        /// <summary>
        /// Access default page.
        /// </summary>
        /// <returns>Default page.</returns>
        public ActionResult Default()
        {
            if (AppSession.CurrentUserMenu == null || AppSession.CurrentUser == null)
            {
                return ForceLogin();
            }

            ViewBag.AccUsername = AppSession.CurrentUser.Username;
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult GettingUserMenu()
        {
            ViewBag.MenuItemList = CurrentSession.MenuItemList;
            return PartialView("~/Views/Home/Menu.cshtml");
        }

        public ActionResult Footer()
        {
            return View();
        }
    }
}