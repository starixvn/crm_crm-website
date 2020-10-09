namespace CRM.Core.Filters
{
    using System;
    using System.Net;
    using Fanex.NAP.Client;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Resources;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CrmAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly INapSession napSession;

        public CrmAuthorizeAttribute() : this(null)
        {
        }

        public CrmAuthorizeAttribute(INapSession napSession)
        {
            this.napSession = napSession ?? NapClientManager.NapSession;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context?.HttpContext == null)
            {
                return;
            }

            if (napSession == null || napSession.UserId <= 0 || string.IsNullOrEmpty(napSession.UserName))
            {
                CloseConnection(context);
                context.Result = new RedirectToActionResult("SignOut", "Auth", null);
            }
        }

        private static void CloseConnection(AuthorizationFilterContext context)
        {
            if (IsAjaxRequest(context.HttpContext.Request))
            {
                var response = context.HttpContext.Response;
                var messageContent = $"<center><b>{ViewResources.UnauthorizeMsg}</b></center>";
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.WriteAsync(messageContent);
            }
            else
            {
                context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        private static bool IsAjaxRequest(HttpRequest request)
           => string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
              string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
    }
}