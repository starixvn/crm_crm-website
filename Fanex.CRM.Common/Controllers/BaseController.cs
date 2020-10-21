namespace Fanex.CRM.Common.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Attributes;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using ONEbook.UM.Filters;

    [OverridableNapSupperAuth]
    [CheckUMFilter]
    public class BaseController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly ILanguageService _langService;

        public BaseController()
        {
            _permissionService = new PermissionService();
            _langService = new LanguageService();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var currentLanguage = _langService.GetCurrentLanguage();
            _langService.SetPageCulture(currentLanguage);
        }

        protected IPermissionService PermissionService => _permissionService;

        public bool HasReadPermission(string functionName)
        {
            return PermissionService.HasReadPermission(functionName);
        }

        public bool HasHostPermission(string functionName)
        {
            return PermissionService.HasHostPermission(functionName);
        }

        public bool HasPermission(string functionName, Constant.Policy policy)
        {
            return PermissionService.HasPermission(functionName, policy);
        }

        public Constant.Policy GetPolicy(string functionName)
        {
            return PermissionService.GetPolicy(functionName);
        }

        protected string ExternalDomain
        {
            get
            {
                var request = HttpContext.Request;
                var domain = string.Format("{0}://{1}{2}{3}", request.Url.Scheme,
                    request.Url.Host,
                    request.Url.IsDefaultPort ? string.Empty : ":" + request.Url.Port,
                    Url.Content("~"));
                domain = domain.Substring(0, domain.Length - 1);
                var mapping = AppSetting.ExternalRedirectMappings
                    .FirstOrDefault(x => x.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase));
                return mapping != null ? mapping.ExternalDomain : string.Empty;
            }
        }
    }
}