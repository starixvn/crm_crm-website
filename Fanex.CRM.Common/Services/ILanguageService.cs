namespace Fanex.CRM.Common.Services
{
    public interface ILanguageService
    {
        string GetCurrentLanguage();

        bool ChangeLanguage(string languageCode);

        void SetPageCulture(string languageCode);
    }
}
