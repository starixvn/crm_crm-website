// --------------------------------------------------------------------------
// <copyright file="SettingManagementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Services.FeedbackSubjectManagement;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Web.Controllers;

    /// <summary>
    /// Setting Management Controller.
    /// </summary>
    public class SettingManagementController : BaseController
    {
        [AuthenticateAction(AuthenticateAction.AuthenticationType.Read, "Index")]
        public ActionResult Index()
        {
            return View("~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Index.cshtml");
        }
    }
}
