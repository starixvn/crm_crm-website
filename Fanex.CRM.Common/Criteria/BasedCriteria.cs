namespace Fanex.CRM.Common.Criteria
{
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class BasedCriteria : CriteriaBase
    {
        public BasedCriteria()
        {
            UserRole = AppSession.CurrentUser.UserRole;
        }

        public UserRole UserRole { get; set; }
    }
}