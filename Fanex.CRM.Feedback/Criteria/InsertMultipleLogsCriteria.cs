namespace Fanex.CRM.Feedback.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Common.Criteria;
    using Common.Helpers;
    using Common.Services;
    using Fanex.CRM.Common.Models;
    using Fanex.Data.Repository;
    using Models;

    public class InsertMultipleLogsCriteria : BasedUpdateCriteria
    {
        public string Subject { get; set; }
        public static int CreatorId => AppSession.CurrentUser.Id;

        [SpParam(SqlParamName = "feedbackdate")]
        public DateTime StartTime { get; set; }

        [SpParam(SqlParamName = "resolutiondate")]
        public DateTime EndTime { get; set; }

        [SpParam(SqlParamName = "stationid")]
        public string ExtensionIds { get; set; }

        public int ResponseId { get; set; }
        public int StatusId { get; set; }

        [SpParam(SqlParamName = "description")]
        public string Remark { get; set; }

        public string Solution { get; set; }
        public bool IsReviewed { get; set; }
        public string Review { get; set; }
        public string ReviewerId { get; set; }
        public string AutoField { get; set; }

        // use in case apply default communication tool
        public string AutoField1 { get; set; }
        public string AutoField2 { get; set; }
        public string AutoField3 { get; set; }

        [SpParam(Ignored = true)]
        public IEnumerable<ProductFilter> Products { get; set; }

        [SpParam(SqlParamName = "product")]
        public string ProductsContent => SerializerHelper.SerializeList(Products);

        [SpParam(Ignored = true)]
        public IEnumerable<CategoryFilter> Categories { get; set; }

        [SpParam(SqlParamName = "category")]
        public string CategoriesContent => SerializerHelper.SerializeList(Categories);

        [SpParam(SqlParamName = "Categorystr")]
        public string CategoryIds { get; set; }

        [SpParam(SqlParamName = "SubCategorystr")]
        public string SubCategoryIds { get; set; }

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

        [SpParam(SqlParamName = "IsCheckedDefaultComm")]
        public bool IsUseDefaultCommunicationTool { get; set; }

        [SpParam(SqlParamName = "CommunicationId")]
        public string CommunicationIds { get; set; }

        public int NoInquiries { get; set; }

        public override string GetSettingKey() => "MultipleLogsInsert";

        public override bool IsValid() => true;
    }
}