namespace Fanex.CRM.Feedback.Criteria
{
    using Common.Services;
    using Data.Repository;
    using Fanex.CRM.Common.Criteria;
    using System;

    public class UpdateMultipleLogsCriteria : UpdateCriteria
    {
        [SpParam(SqlParamName = "listfeedbackid")]
        public string ListFeedbackId { get; set; }

        [SpParam(SqlParamName = "countryid")]
        public int CountryId { get; set; }

        [SpParam(SqlParamName = "siteid")]
        public int SiteId { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        [SpParam(SqlParamName = "languageid")]
        public int LanguageId { get; set; }

        [SpParam(SqlParamName = "feedbackdate")]
        public DateTime StartTime { get; set; }

        [SpParam(SqlParamName = "resolutiondate")]
        public DateTime EndTime { get; set; }

        [SpParam(SqlParamName = "communicationid")]
        public int CommunicationToolId { get; set; }

        [SpParam(SqlParamName = "stationid")]
        public int ExtensionId { get; set; }

        [SpParam(SqlParamName = "maincategoryid")]
        public int MainCategoryId { get; set; }

        [SpParam(SqlParamName = "subcategoryid")]
        public int SubCategoryId { get; set; }

        [SpParam(SqlParamName = "roleid")]
        public int RoleId { get; set; }

        [SpParam(SqlParamName = "productid")]
        public int ProductId { get; set; }

        [SpParam(SqlParamName = "subproductid")]
        public int SubProductId { get; set; }

        [SpParam(SqlParamName = "bettypeid")]
        public int BetTypeId { get; set; }

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

        public int ReviewerId
        {
            get
            {
                return IsReviewed ? AppSession.CurrentUser.Id : 0;
            }
        }

        public int UpdateReviewedMode { get; set; }

        public override string GetSettingKey()
        {
            return "MultipleLogsUpdate";
        }
    }
}
