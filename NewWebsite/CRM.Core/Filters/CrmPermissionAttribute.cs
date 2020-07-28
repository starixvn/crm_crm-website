namespace CRM.Core.Filters
{
    using System;
    using System.Threading.Tasks;
    using Fanex.NAP.Client;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.Core.Business;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CrmPermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IUserService userService;
        private readonly INapSettings napSettings;
        private readonly INapSession napSession;

        public CrmPermissionAttribute() : this(null, null, null)
        {
        }

        public CrmPermissionAttribute(IUserService userService, INapSettings napSettings, INapSession napSession)
        {
            this.userService = userService ?? new UserService();
            this.napSettings = napSettings ?? NapClientManager.NapSettings;
            this.napSession = napSession ?? NapClientManager.NapSession;
        }

        public string FunctionKey { get; set; }
        public Constant.Policy Policy { get; set; }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context?.HttpContext == null)
            {
                return;
            }

            int policy = await userService.GetPermissionAsync(napSession.UserId, napSettings.SiteId, FunctionKey);
            if (policy < (int)Policy)
            {
                context.Result = new RedirectToActionResult("405", "error", null);
            }
        }
    }
}