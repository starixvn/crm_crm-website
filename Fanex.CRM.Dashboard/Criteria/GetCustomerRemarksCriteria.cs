// --------------------------------------------------------------------------
// <copyright file="GetCustomerRemarksCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Customer Dashboard Information Criteria.
    /// </summary>
    public class GetCustomerRemarksCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_Dashboard_GetCustomerRemarks";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
