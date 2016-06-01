using System;
using System.Web.Mvc;
using System.Web.Routing;
using Fanex.CRM.Common.Services;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.CRM.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class OverridableNapSupperAuthAttribute : NapSuperAuthAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (AppSession.CurrentUserMenu == null || AppSession.CurrentUser == null)
            {
                var routeValue = new
                {
                    controller = "UserLogin",
                };

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValue));
            }
        }
    }
}