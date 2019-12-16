namespace Fanex.CRM.Feedback.Criteria
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.CRM.Common.Models;
    using Fanex.Data.Repository;

    public class CheckNotExistCommunicationToolSettingCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public IEnumerable<BaseModel> Customers { get; set; } = new List<BaseModel>();

        [SpParam(SqlParamName = "ListCustId")]
        public string CustomerXml
        {
            get
            {
                var xml = new StringBuilder("<ROOT>");
                foreach (BaseModel customer in Customers)
                {
                    xml.AppendFormat("<Cust CustID='{0}' Username='{1}' />", customer.Id, customer.Name);
                }
                xml.Append("</ROOT>");
                return xml.ToString();
            }
        }

        [SpParam(SqlParamName = "Categorystr")]
        public string CategoryIds { get; set; }

        [SpParam(SqlParamName = "SubCategorystr")]
        public string SubCategoryIds { get; set; }

        public override string GetSettingKey() => "Acc_CRM_GuestFeedbackCheck_Multiple";

        public override bool IsValid() => true;
    }
}