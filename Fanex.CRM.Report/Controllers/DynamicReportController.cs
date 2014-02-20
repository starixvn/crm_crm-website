// --------------------------------------------------------------------------
// <copyright file="DynamicReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Dynamic Report Controller.
    /// </summary>
    public class DynamicReportController : BaseController
    {
        [NapAuth(FunctionKey = FunctionPermission.CRMReport, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Report/DynamicReport/Index.cshtml");
        }
    }
}
