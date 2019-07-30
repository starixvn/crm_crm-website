namespace Fanex.CRM.Common.Criteria
{
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class BasedLanguageCriteria : CriteriaBase
    {
        public BasedLanguageCriteria()
        {
            UserRole = AppSession.CurrentUser.UserRole;
            Lang = AppSession.CurrentUser.Language ?? "en-US";
        }

        public UserRole UserRole { get; set; }
        public string Lang { get; set; }
    }
}