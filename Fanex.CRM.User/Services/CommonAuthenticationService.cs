namespace Fanex.CRM.User.Services
{
    using Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.User.Helpers;
    using NAP.Core.Configuration;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using System.Web;
    using System.Web.Security;

    public class CommonAuthenticationService : ICommonAuthenticationService
    {
        private readonly ISiteService _siteService;
        private readonly ISecurityService _securityService;
        private readonly IMaintenanceService _maintenanceService;

        public CommonAuthenticationService()
            : this(null, null, null)
        {
        }

        public CommonAuthenticationService(
            ISiteService siteService,
            ISecurityService securityService,
            IMaintenanceService maintenanceService)
        {
            _siteService = siteService ?? new SiteService();
            _securityService = securityService ?? new SecurityService();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        public RecheckInStatus RecheckIn(CRMUser user)
        {
            var settings = NapSettings.Settings;

            if (_maintenanceService.IsUnderMaintenance())
            {
                return RecheckInStatus.UnderMaintenance;
            }

            if (!settings.EnableReCheckIn || user.OnUserId <= 0)
            {
                return RecheckInStatus.Success;
            }

            if (!_siteService.IsActivated(settings.SiteId))
            {
                return RecheckInStatus.InvalidSite;
            }

            int status = _securityService.ReCheckIn(user.OnUserId, settings.SiteId);

            return status > 0 ? RecheckInStatus.KickOut : RecheckInStatus.Success;
        }

        public void Logout(CRMUser user)
        {
            if (user != null && user.OnUserId > 0)
            {
                _securityService.Logout(user.OnUserId);

            }

            AuthencationHelper.ClearCookies();
            FormsAuthentication.SignOut();
        }
    }
}