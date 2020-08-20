namespace Fanex.CRM.Common.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class XFrameOptionAttribute : ActionFilterAttribute
    {
        private const string XFrameOptionHeaderName = "X-Frame-Options";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AddXFrameOption(filterContext);
            base.OnActionExecuting(filterContext);
        }

        private static void AddXFrameOption(ActionExecutingContext actionContext)
        {
            IEnumerable<string> headerValues = actionContext.HttpContext.Response.Headers.GetValues(XFrameOptionHeaderName);

            if (headerValues != null && headerValues.Any())
            {
                actionContext.HttpContext.Response.Headers.Remove(XFrameOptionHeaderName);
            }
        }
    }
}