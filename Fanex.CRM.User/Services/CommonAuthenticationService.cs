namespace Fanex.CRM.User.Services
{
    using Enumerations;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.User.Helpers;
    using Fanex.NAP.Core.Models.ValueObject;
    using NAP.Core.Configuration;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using System.Web.Security;
    using System.Linq;
    public class CommonAuthenticationService : ICommonAuthenticationService
    {
        private readonly ISiteService _siteService;
        private readonly ISecurityService _securityService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IUserService _userService;
        public CommonAuthenticationService()
            : this(null, null, null, null)
        {
        }

        public CommonAuthenticationService(
            ISiteService siteService,
            ISecurityService securityService,
            IMaintenanceService maintenanceService,
            IUserService userService
            )
        {
            _siteService = siteService ?? new SiteService();
            _securityService = securityService ?? new SecurityService();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
            _userService = userService ?? new UserService();
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

            var status = _securityService.ReCheckIn(user.OnUserId, settings.SiteId);

            return status > 0 ? RecheckInStatus.KickOut : RecheckInStatus.Success;
        }

        public void Logout(CRMUser user)
        {
            if (user?.OnUserId > 0)
            {
                _securityService.Logout(user.OnUserId);
            }

            AuthencationHelper.ClearCookies();
            FormsAuthentication.SignOut();
        }

        public UserRole GetUserRole(int userID)
        {

            if (NapSettings.Settings.SiteId.ToString() == AppSetting.CN88SiteId)
            {
                return UserRole.CN88Staff;
            }

            if (NapSettings.Settings.SiteId.ToString() == AppSetting.SiteId)
            {
                return UserRole.NormalStaff;
            }

            return UserRole.None;
        }
    }
}