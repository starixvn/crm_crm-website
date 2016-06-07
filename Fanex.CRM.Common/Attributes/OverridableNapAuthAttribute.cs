using System.Web;
using System.Web.Mvc;
using Fanex.CRM.Infrastructure.Constants;
using Fanex.CRM.Infrastructure.Resources;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.CRM.Common.Attributes
{
    public class OverridableNapAuthAttribute : NapAuthAttribute
    {
        protected override void HandleForbiddenAjaxRequest(AuthorizationContext filterContext, HttpResponseBase response)
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
    } 
}