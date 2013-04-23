
namespace Fanex.CRM.DataAccess.Interface
{
    using System.Collections.Generic;
    using Fanex.CRM.Entities;

    public interface ISecurityDataAccess
    {
        List<Security> GetCustomerRoles(int custId);
    }
}
