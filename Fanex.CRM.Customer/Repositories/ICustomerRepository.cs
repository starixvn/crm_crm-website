// --------------------------------------------------------------------------
// <copyright file="ICustomerRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

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

        Result UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria);

        IEnumerable<string> GetCustomersByUsername(CustomerSuggestionCriteria criteria);

        IEnumerable<RelatedCustomerSuggestion> GetRelatedCustomerAutoSuggestions(RelatedCustomerAutoSuggestionCriteria criteria);

        IEnumerable<BaseModel> VerifyUsername(VerifyRelatedCustomerCriteria criteria);
    }
}
