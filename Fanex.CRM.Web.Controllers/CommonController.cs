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

    /// <summary>
    /// Common Controller.
    /// </summary>
    public class CommonController : BaseController
    {
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}