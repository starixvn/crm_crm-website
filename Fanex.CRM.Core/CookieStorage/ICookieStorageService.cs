namespace Fanex.CRM.Infrastructure.CookieStorage
{
    using System;

    public interface ICookieStorageService
    {
        void Add(string key, string value, DateTime expires);
        string Get(string key);
    }
}
