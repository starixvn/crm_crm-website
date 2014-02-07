// --------------------------------------------------------------------------
// <copyright file="RedemptionController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// Redemption Controller.
    /// </summary>
    public class RedemptionController : BaseController
    {
        public RedemptionController()
        {
        }

        [NapAuth(FunctionKey = FunctionPermission.RedemptionManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/Redemption/ViewLog/Index.cshtml");
        }

        [ActionName("New")]
        public ActionResult InsertRedemption()
        {
            return View("~/Areas/Redemption/InsertRedemption.cshtml");
        }

        [ActionName("edit")]
        public ActionResult EditRedemption()
        {
            return View("~/Areas/Redemption/EditRedemption.cshtml");
        }
    }
}
