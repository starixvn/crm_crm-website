namespace Fanex.CRM.Common.Criteria
{
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class LanguageCriteria : CriteriaBase
    {
        public LanguageCriteria()
        {
            Lang = AppSession.CurrentUser.Language ?? "en-US";
        }

        public string Lang { get; set; }
    }
}