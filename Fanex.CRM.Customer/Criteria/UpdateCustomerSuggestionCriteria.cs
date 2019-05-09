// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerSuggestionCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Customer Information By Username Criteria.
    /// </summary>
    public class UpdateCustomerSuggestionCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "userid")]
        public int UserId { get; set; }

        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "username")]
        public string Username { get; set; }

        public int UserRole { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfo_UpdateCustomerSuggestion";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
