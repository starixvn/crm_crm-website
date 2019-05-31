using System.Globalization;
using System.Threading;

namespace Fanex.CRM.Common.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly string _defaultLanguage = "en-US";

        public string GetCurrentLanguage()
        {
            var userSession = AppSession.CurrentUser;
            if (userSession != null && !string.IsNullOrEmpty(userSession.SettingLanguage))
            {
                return userSession.SettingLanguage;
            }

            return _defaultLanguage;
        }

        public void SetPageCulture(string languageCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
        }

        public bool ChangeLanguage(string languageCode)
        {
            if (!string.IsNullOrEmpty(languageCode))
            {
                var updated = AppSession.UpdateLanguage(languageCode);
                if (updated)
                {
                    this.SetPageCulture(languageCode);
                    return true;
                }
            }

            return false;
        }
    }
}
