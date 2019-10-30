namespace Fanex.CRM.AdminTool.Criteria.CommunicationToolSetting
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class CreateCommunicationToolSettingCriteria : CriteriaBase
    {
        public static int UserId => AppSession.CurrentUser.Id;
        public static string IP => IPHelper.ClientIP;

        [SpParam(SqlParamName = "Category")]
        public string CategoryIds { get; set; }

        [SpParam(SqlParamName = "SubCategory")]
        public string SubCategoryIds { get; set; }

        [SpParam(SqlParamName = "Comm")]
        public string CommunicationIds { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public IEnumerable<BaseModel> Customers { get; set; } = new List<BaseModel>();

        [SpParam(SqlParamName = "ListCustId")]
        public string CustomerXml
        {
            get
            {
                var xml = new StringBuilder("<ROOT>");
                foreach (BaseModel cust in Customers)
                {
                    xml.AppendFormat("<Cust CustID='{0}' Username='{1}' />", cust.Id, cust.Name);
                }
                xml.Append("</ROOT>");
                return xml.ToString();
            }
        }

        public override string GetSettingKey() => "Acc_CRM_DefaultCommToolIns";

        public override bool IsValid() => true;
    }
}