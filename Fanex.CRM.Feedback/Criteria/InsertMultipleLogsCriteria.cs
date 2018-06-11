namespace Fanex.CRM.Feedback.Criteria
{
    using Common.Criteria;
    using Common.Helpers;
    using Common.Services;
    using Fanex.Data.Repository;
    using Models;
    using System;

    public class InsertMultipleLogsCriteria : UpdateCriteria
    {
        [SpParam(SqlParamName = "username")]
        public string ListCustomers { get; set; }

        [SpParam(SqlParamName = "countryid")]
        public int CountryId { get; set; }

        [SpParam(SqlParamName = "siteid")]
        public int SiteId { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        [SpParam(SqlParamName = "languageid")]
        public int LanguageId { get; set; }

        [SpParam(SqlParamName = "creatorid")]
        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "feedbackdate")]
        public DateTime StartTime { get; set; }

        [SpParam(SqlParamName = "resolutiondate")]
        public DateTime EndTime { get; set; }

        [SpParam(SqlParamName = "communicationid")]
        public string CommunicationIds { get; set; }

        [SpParam(SqlParamName = "stationid")]
        public string ExtensionIds { get; set; }

        [SpParam(SqlParamName = "roleid")]
        public int RoleId { get; set; }

        [SpParam(SqlParamName = "responseid")]
        public int ResponseId { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        [SpParam(SqlParamName = "description")]
        public string Remark { get; set; }

        [SpParam(SqlParamName = "solution")]
        public string Solution { get; set; }

        [SpParam(SqlParamName = "licenseetype")]
        public int LicenseeTypeId { get; set; }

        public bool IsReviewed { get; set; }

        public string Review { get; set; }

        public string ReviewerId { get; set; }

        [SpParam(SqlParamName = "autofield")]
        public string AutoField { get; set; }

        [SpParam(Ignored = true)]
        public System.Collections.Generic.IEnumerable<ProductFilter> Products { get; set; }

        [SpParam(SqlParamName = "product")]
        public string ProductsContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Products);
            }
        }

        [SpParam(Ignored = true)]
        public System.Collections.Generic.IEnumerable<CategoryFilter> Categories { get; set; }

        [SpParam(SqlParamName = "category")]
        public string CategoriesContent
        {
            get
            {
                return SerializerHelper.SerializeList(this.Categories);
            }
        }

        public override string GetSettingKey()
        {
            return "MultipleLogsInsert";
        }
    }
}
