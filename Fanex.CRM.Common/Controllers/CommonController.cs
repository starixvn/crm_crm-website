// --------------------------------------------------------------------------
// <copyright file="CommonController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;

    /// <summary>
    /// Common Controller.
    /// </summary>
    [AllowAnonymous]
    public class CommonController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;

        public CommonController()
            : this(null)
        {
        }

        public CommonController(MaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        /// <summary>
        /// The denied access.
        /// </summary>
        /// <returns>The denied access result.</returns>
        public ActionResult AccessDenied()
        {
            return View("~/Views/Common/AccessDenied.cshtml");
        }

        public ActionResult ErrorPage()
        {
            return View("~/Views/Common/ErrorPage.cshtml");
        }

        /// <summary>
        /// Checking down time.
        /// </summary>
        /// <returns>Return result.</returns>
        public ActionResult Offline()
        {
            var downTime = _maintenanceService.GetUnderMaintenanceTime();
            if (!downTime.IsUnderMaintenanceTime)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            ViewBag.UmtFrom = downTime.From;
            ViewBag.UmtTo = downTime.To;

            return View();
        }
    }
}