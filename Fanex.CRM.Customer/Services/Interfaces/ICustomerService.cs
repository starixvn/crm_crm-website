namespace Fanex.CRM.Customer.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;

    public interface ICustomerService
    {
        CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria);

        IEnumerable<BaseCustomer> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria);

        bool UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria);

        CommandResult UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria);

        IEnumerable<string> QueryCustomerByUsername(CustomerSuggestionCriteria criteria);

        IEnumerable<RelatedCustomer> VerifyUsername(VerifyRelatedCustomerCriteria criteria);
    }
}