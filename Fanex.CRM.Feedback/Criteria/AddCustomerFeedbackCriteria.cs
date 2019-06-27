namespace Fanex.CRM.Feedback.Criteria
{
    using Fanex.CRM.Feedback.Models;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;

    public class AddCustomerFeedbackCriteria : AddGuestFeedbackCriteria
    {
        public bool OverrideCRMInfo { get; set; }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public override Hashtable GetSpParams()
        {
            var param = base.GetSpParams();
            param.Remove("UserRole");
            param["siteid"] = CustomerInfo.SiteSetting.Id;
            param.Add("custid", CustomerInfo.Id);
            param["roleid"] = Role.Id;
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
