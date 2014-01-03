// --------------------------------------------------------------------------
// <copyright file="AuthenticateRead.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers.CommonFilters
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using Fanex.CRM.Web.Controllers.Helper;

    /// <summary>
    /// Authenticate Read.
    /// </summary>
    public class AuthenticateRead : FilterAttribute, IActionFilter
    {
        private readonly IPermissionService _permissionService;

        public AuthenticateRead()
            : this(null)
        {
        }

        public AuthenticateRead(IPermissionService permissionService)
        {
            _permissionService = permissionService ?? new PermissionService();
        }

        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.ActionName;
            var controlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var pathActionName = controlName + "/" + actionName;

            bool hasPermissionRead = _permissionService.HasPermission(pathActionName, AppSession.PageAccess);

            if (!hasPermissionRead)
            {
                AccessDenied(filterContext);
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        private static void AccessDenied(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = "Common",
                action = "AccessDenied"
            }));
        }
    }
}