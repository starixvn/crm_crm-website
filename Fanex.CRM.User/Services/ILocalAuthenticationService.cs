namespace Fanex.CRM.User.Services
{
    using Enumerations;
    using Models;
    using NAP.Core.Web.Session;

    public interface ILocalAuthenticationService
    {
        LoginStatus Login(LocalLogin localLogin, ICurrentSession napCurrentSession);
    }
}
