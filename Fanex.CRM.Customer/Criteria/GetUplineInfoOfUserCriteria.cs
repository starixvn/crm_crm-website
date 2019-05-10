// --------------------------------------------------------------------------
// <copyright file="GetUplineInfoOfUserCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using Common.Enumerations;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Customer Information By Username Criteria.
    /// </summary>
    public class GetUplineInfoOfUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public UserRole UserRole { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfo_GetUplineList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
