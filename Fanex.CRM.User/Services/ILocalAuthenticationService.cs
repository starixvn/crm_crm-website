namespace Fanex.CRM.User.Services
{
    using Enumerations;
    using Models;

    public interface ILocalAuthenticationService
    {
        LoginStatus Login(LocalLogin localLogin);
    }
}
