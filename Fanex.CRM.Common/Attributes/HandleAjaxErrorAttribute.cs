namespace Fanex.CRM.Common.Attributes
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Resources;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class HandleAjaxErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            // if the request is AJAX return JSON else view.
            var requestHeaderType = filterContext.HttpContext.Request.Headers["X-Requested-With"];
            if (requestHeaderType == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = (int)HttpStatusCode.InternalServerError,
                        message = Labels.Error_Server,
                        innerException = filterContext.Exception.ToString()
                    }
                };
            }
            else
            {
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                filterContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}