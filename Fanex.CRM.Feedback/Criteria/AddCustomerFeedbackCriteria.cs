// --------------------------------------------------------------------------
// <copyright file="AddCustomerFeedbackCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Criteria
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Feedback.Models;

    /// <summary>
    /// Add Customer Feedback Criteria.
    /// </summary>
    public class AddCustomerFeedbackCriteria : AddGuestFeedbackCriteria
    {
        public bool OverrideCRMInfo { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public override Hashtable GetSpParams()
        {
            var param = base.GetSpParams();
            param["siteid"] = CustomerInfo.SiteSetting.Id;
            param.Add("custid", CustomerInfo.Id);
            param.Add("roleid", Role.Id);
            param["relatedcustid"] = HasRelated && !string.IsNullOrWhiteSpace(RelatedCustomerId) ?
                RelatedCustomerId :
                null;
            param.Add("isoverwrite", OverrideCRMInfo);
            return param;
        }

        public override string GetSettingKey()
        {
            return "CustomerFeedbackIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
