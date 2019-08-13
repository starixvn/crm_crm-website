namespace Fanex.CRM.Feedback.Criteria
{
    using System.Collections;
    using Fanex.CRM.Feedback.Models;

    public class AddCustomerFeedbackCriteria : AddGuestFeedbackCriteria
    {
        public bool OverrideCRMInfo { get; set; }

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

        public override bool IsValid() => true;
    }
}