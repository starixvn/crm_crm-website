// --------------------------------------------------------------------------
// <copyright file="GetDynamicReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Communication Logs By Customer Identity Criteria.
    /// </summary>
    public class GetDynamicReportCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "reporttype")]
        public int ReportType { get; set; }

        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "productid")]
        public int ProductId { get; set; }

        [SpParam(SqlParamName = "siteid")]
        public int SiteId { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        [SpParam(SqlParamName = "responseid")]
        public int ResponseId { get; set; }

        [SpParam(SqlParamName = "roleid")]
        public int RoleId { get; set; }

        [SpParam(SqlParamName = "communicationtoolid")]
        public int CommunicationToolId { get; set; }

        [SpParam(SqlParamName = "maincategoryid")]
        public int MainCategoryId { get; set; }

        [SpParam(SqlParamName = "subcategoryid")]
        public int SubCategoryId { get; set; }

        [SpParam(SqlParamName = "handler")]
        public string Handler { get; set; }

        [SpParam(SqlParamName = "reviewer")]
        public string Reviewer { get; set; }

        [SpParam(SqlParamName = "extensionid")]
        public int ExtensionId { get; set; }

        public override string GetSettingKey()
        {
            return "Report_DynamicReport_GetReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
