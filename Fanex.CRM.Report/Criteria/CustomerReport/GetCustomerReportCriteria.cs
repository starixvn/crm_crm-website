// --------------------------------------------------------------------------
// <copyright file="GetCustomerReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.CustomerReport
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Customer  Report Criteria.
    /// </summary>
    public class GetCustomerReportCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "username")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "languageid")]
        public string LanguageIds { get; set; }

        [SpParam(SqlParamName = "siteid")]
        public string SiteIds { get; set; }

        [SpParam(SqlParamName = "frombirthday")]
        public DateTime? FromDateBirthDay { get; set; }

        [SpParam(SqlParamName = "tobirthday")]
        public DateTime? ToDateBirthDay { get; set; }

        [SpParam(SqlParamName = "titleid")]
        public int TitleId { get; set; }

        [SpParam(SqlParamName = "vip")]
        public int VIPTypeId { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGetFullReport { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGetFromCached { get; set; }

        [SpParam(Ignored = true)]
        public string SortField { get; set; }

        [SpParam(Ignored = true)]
        public string SortDirection { get; set; }

        [SpParam(Ignored = true)]
        public bool IsSortReport { get; set; }

        public override string GetSettingKey()
        {
            return "Report_CustomerReport_GetReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
