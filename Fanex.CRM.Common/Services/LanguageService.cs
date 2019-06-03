using System.Globalization;
using System.Threading;

namespace Fanex.CRM.Common.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly string _defaultLanguage = "en-US";

        public string GetCurrentLanguage()
        {
            var settingLanguage = AppSession.SettingLanguage;
            if (settingLanguage != null)
            {
                return settingLanguage;
            }

            return _defaultLanguage;
        }

        public void SetPageCulture(string languageCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
        }

        public void ChangeLanguage(string languageCode)
        {
            AppSession.SettingLanguage = languageCode;
        }
    }
}
