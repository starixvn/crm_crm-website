// --------------------------------------------------------------------------
// <copyright file="CommonController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Resources;

    /// <summary>
    /// Common Controller.
    /// </summary>
    public class CommonController : BaseController
    {
        /// <summary>
        /// The denied access.
        /// </summary>
        /// <returns>The denied access result.</returns>
        public ActionResult AccessDenied()
        {
            var previousHttpMethod = AppSession.CurrentHttpMethod;
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
    }
}