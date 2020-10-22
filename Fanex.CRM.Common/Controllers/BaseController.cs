namespace Fanex.CRM.Common.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Configuration.ExternalRedirectConfigs;
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
                var roorUrl = Url.Content("~");
                roorUrl = roorUrl.EndsWith("/") ? roorUrl.Substring(0, roorUrl.Length - 1) : roorUrl;
                var config = string.Empty;
                SerilogHelper.WriteInformation(request.Url.AbsoluteUri);
                foreach (ExternalRedirectElement element in ExternalRedirectConfigs.GetConfigs())
                {
                    SerilogHelper.WriteInformation(element.DomainUri.AbsoluteUri);
                    var localPath = element.DomainUri.LocalPath.EndsWith("/")
                           ? element.DomainUri.LocalPath.Substring(0, element.DomainUri.LocalPath.Length - 1)
                           : element.DomainUri.LocalPath;
                    if (element.DomainUri.Host.Equals(request.Url.Host, StringComparison.OrdinalIgnoreCase)
                        && (!element.IncludePort || request.Url.Port == element.DomainUri.Port)
                        && localPath.Equals(roorUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        config = element.ExternalDomain;
                    }
                }

                return config;
            }
        }
    }
}