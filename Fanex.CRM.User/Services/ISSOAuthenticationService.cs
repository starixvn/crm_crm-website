namespace Fanex.CRM.User.Services
{
    using Enumerations;
    using Models;
    using NAP.Core.Web.Session;

    public interface ISSOAuthenticationService
    {
        LoginStatus SingleSignOn(SSOLogin ssoLogin, ICurrentSession napCurrentSession);
    }
}
