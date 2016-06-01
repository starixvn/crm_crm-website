using Fanex.CRM.Common.Services;
using Fanex.CRM.Infrastructure.Constants;
using Fanex.CRM.Infrastructure.Resources;
using Fanex.NAP.Core.Web.Attribute;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fanex.CRM.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class OverridableNapSupperAuthAttribute : NapSuperAuthAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAuthenticated
                || AppSession.CurrentUserMenu == null || AppSession.CurrentUser == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            Code = ErrorCodeConstants.NoPermission,
                            Message = Labels.NoPermission
                        }
                    };

                    filterContext.HttpContext.Response.StatusCode = 403;
                }
                else
                {
                    var routeValue = new
                    {
                        controller = "Utility",
                        action = "AccessDenied",
                        message = "AccessDenied"
                    };

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValue));
                }
            }


            base.OnAuthorization(filterContext);

        }
    }
}
