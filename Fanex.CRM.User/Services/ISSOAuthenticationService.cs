namespace Fanex.CRM.User.Services
{
    using Enumerations;
    using Models;
    public interface ISSOAuthenticationService
    {
        LoginStatus SingleSignOn(SSOLogin ssoLogin);
    }
}
