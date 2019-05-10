using Fanex.CRM.Common.Enumerations;
using Fanex.CRM.Common.Services;

namespace Fanex.CRM.Common.Criteria
{
    public class BasedUpdateCriteria : UpdateCriteria
    {
        public BasedUpdateCriteria()
        {
            UserRole = AppSession.CurrentUser.UserRole;
        }

        public UserRole UserRole { get; set; }
    }
}
