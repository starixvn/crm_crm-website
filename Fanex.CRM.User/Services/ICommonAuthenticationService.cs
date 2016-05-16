namespace Fanex.CRM.User.Services
{
    using Enumerations;
    using Fanex.NAP.Core.Web.Session;

    public interface ICommonAuthenticationService
    {
        RecheckInStatus RecheckIn(ICurrentSession currentSession);

        void Logout(ICurrentSession currentSession);
    }
}
