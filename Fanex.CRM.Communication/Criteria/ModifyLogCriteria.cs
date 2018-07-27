namespace Fanex.CRM.Communication.Criteria
{
    using Fanex.CRM.Common.Criteria;
    using Fanex.Data.Repository;
    using System;

    /// <summary>
    /// Modify Criteria.
    /// </summary>
    public class ModifyLogCriteria : UpdateCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "relatedcustid")]
        public string RelatedCustomerId { get; set; }

        [SpParam(SqlParamName = "languageid")]
        public int LanguageId { get; set; }

        [SpParam(SqlParamName = "countryid")]
        public int CountryId { get; set; }

        [SpParam(SqlParamName = "feedbackdate")]
        public DateTime StartTime { get; set; }

        [SpParam(SqlParamName = "resolutiondate")]
        public DateTime EndTime { get; set; }

        [SpParam(SqlParamName = "siteid")]
        public int SiteId { get; set; }

        [SpParam(SqlParamName = "roleid")]
        public int RoleId { get; set; }

        [SpParam(SqlParamName = "responseid")]
        public int ResponseId { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        [SpParam(SqlParamName = "licenseetype")]
        public int LicenseeTypeId { get; set; }

        [SpParam(SqlParamName = "description")]
        public string Remark { get; set; }

        [SpParam(SqlParamName = "solution")]
        public string Solution { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
