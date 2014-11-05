// --------------------------------------------------------------------------
// <copyright file="CommonController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Resources;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;

    /// <summary>
    /// Common Controller.
    /// </summary>
    public class CommonController : BaseController
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
            var previousHttpMethod = string.IsNullOrEmpty(AppSession.CurrentHttpMethod) ?
                HttpMethodConstants.Get :
                AppSession.CurrentHttpMethod;
            switch (previousHttpMethod)
            {
                case HttpMethodConstants.Get:
                    return View("~/Views/Common/AccessDenied.cshtml");
                default:
                    var errorInfo = new Result
                    {
                        Code = ErrorCodeConstants.NoPermission,
                        Message = Labels.NoPermission
                    };
                    return Json(errorInfo, JsonRequestBehavior.AllowGet);
            }
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
            var isCheckUM = NAP.Core.Configuration.NapSettings.Settings.EnableCheckUM;
            if (!isCheckUM)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            var downTime = _maintenanceService.GetDowntimeInfo();
            if (!downTime.IsDowntime)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            ViewBag.UmtFrom = downTime.From;
            ViewBag.UmtTo = downTime.To;

            return View();
        }
    }
}