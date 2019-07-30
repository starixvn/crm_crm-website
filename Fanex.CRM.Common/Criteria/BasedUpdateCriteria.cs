namespace Fanex.CRM.Common.Criteria
{
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Services;

    public class BasedUpdateCriteria : UpdateCriteria
    {
        public BasedUpdateCriteria()
        {
            UserRole = AppSession.CurrentUser.UserRole;
        }

        public UserRole UserRole { get; set; }
    }
}