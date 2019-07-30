namespace Fanex.CRM.Common.Attributes
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Resources;
    using Fanex.NAP.Core.Web.Attribute;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OverridableNapAuthAttribute : NapAuthAttribute
    {
        protected override void HandleForbiddenAjaxRequest(AuthorizationContext filterContext, HttpResponseBase response)
        {
            filterContext.Result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    Code = (int)HttpStatusCode.Forbidden,
                    Message = Labels.NoPermission
                }
            };

            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
    }
}