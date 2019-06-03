namespace Fanex.CRM.Common.Services
{
    public interface ILanguageService
    {
        string GetCurrentLanguage();

        void ChangeLanguage(string languageCode);

        void SetPageCulture(string languageCode);
    }
}
