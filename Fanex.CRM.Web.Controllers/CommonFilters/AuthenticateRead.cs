namespace Fanex.CRM.Web.Controllers.CommonFilters
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using Fanex.CRM.Web.Controllers.Helper;

    public class AuthenticateRead : FilterAttribute, IActionFilter
    {
        private readonly IPermissionService _permissionService;
        public AuthenticateRead() : this(null) { }
        public AuthenticateRead(IPermissionService permissionService)
        {
            _permissionService = permissionService ?? new PermissionService();
        }

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
            //throw new NotImplementedException();
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