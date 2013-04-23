namespace Fanex.CRM.Web.Controllers.CommonFilters
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using Fanex.CRM.Web.Controllers.Helper;

    public class AuthenticateHost : FilterAttribute, IActionFilter
    {
        private readonly IPermissionService _permissionService;
        public AuthenticateHost() : this(null) { }
        public AuthenticateHost(IPermissionService permissionService)
        {
            _permissionService = permissionService ?? new PermissionService();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var actionName = filterContext.ActionDescriptor.ActionName;
            var controlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var pathActionName = controlName + "/" + actionName;

            bool hasPermissionHost = _permissionService.GetHostPermission(pathActionName, AppSession.PageAccess);

            if (!hasPermissionHost)
            {
                UpdateActionDenied(filterContext);
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }

        private static void UpdateActionDenied(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = "Common",
                action = "UpdateActionDenied"
            }));
        }
    }
}