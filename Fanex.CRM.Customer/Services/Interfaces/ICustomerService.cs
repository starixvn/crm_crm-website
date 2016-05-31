// --------------------------------------------------------------------------
// <copyright file="ICustomerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface ICustomerService
    {
        CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria);

        IEnumerable<BaseCustomer> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria);

        bool UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria);

        Result UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria);

        IEnumerable<string> QueryCustomerByUsername(CustomerSuggestionCriteria criteria);

        IEnumerable<BaseModel> VerifyUsername(VerifyRelatedCustomerCriteria criteria);
    }
}
