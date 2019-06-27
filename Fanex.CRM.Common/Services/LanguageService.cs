using System.Globalization;
using System.Threading;

namespace Fanex.CRM.Common.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly string _defaultLanguage = "en-US";

        public string GetCurrentLanguage()
        {
            var currentUser = AppSession.CurrentUser;
            if (currentUser != null && currentUser.Language != null)
            {
                return currentUser.Language;
            }

            return _defaultLanguage;
        }

        public void SetPageCulture(string languageCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
        }

        public void ChangeLanguage(string languageCode)
        {
            AppSession.UpdateLanguage(languageCode);
        }
    }
}
