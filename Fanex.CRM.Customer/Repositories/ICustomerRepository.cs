namespace Fanex.CRM.Customer.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface ICustomerRepository
    {
        CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria);

        IEnumerable<CustomerInfo> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria);

        bool UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria);

        IEnumerable<BaseModel> VerifyUsername(VerifyRelatedCustomerCriteria criteria);
    }
}