namespace Fanex.CRM.User.Services
{
    using Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.NAP.Core.Web.Session;

    public interface ICommonAuthenticationService
    {
        RecheckInStatus RecheckIn(CRMUser user);

        void Logout(CRMUser user);
    }
}
