// --------------------------------------------------------------------------
// <copyright file="VerifyRelatedCustomerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Verify Related Customer Criteria.
    /// </summary>
    public class VerifyRelatedCustomerCriteria : CriteriaBase
    {
        [SpParamAttribute(SqlParamName = "username")]
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "CustomerVerify";
        }
    }
}
