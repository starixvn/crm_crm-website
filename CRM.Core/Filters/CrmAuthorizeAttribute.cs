namespace CRM.Core.Filters
{
    using System;
    using Fanex.NAP.Client;
    using Fanex.NAP.Client.Helpers.Web;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
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
                context.Result = new RedirectToActionResult("SignOut", "Auth", null);
            }
        }
    }
}