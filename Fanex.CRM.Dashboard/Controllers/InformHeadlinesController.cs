// --------------------------------------------------------------------------
// <copyright file="InformHeadlinesController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Web.Controllers;

    /// <summary>
    /// Inform Headlines Controller.
    /// </summary>
    public class InformHeadlinesController : BaseController
    {
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "ViewInforms")]
        public ActionResult ViewInforms()
        {
            return View("~/Areas/Customer/InformHeadlines/ViewInforms/Index.cshtml");
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "ViewInforms")]
        public ActionResult OpenEditInformView()
        {
            return View("~/Areas/Customer/InformHeadlines/EditInform/Index.cshtml");
        }

        [AuthenticateAction(AuthenticateAction.AuthenticationType.Host, "ViewInforms")]
        public ActionResult OpenCreateInformView()
        {
            return View("~/Areas/Customer/InformHeadlines/CreateInform/Index.cshtml");
        }
    }
}
