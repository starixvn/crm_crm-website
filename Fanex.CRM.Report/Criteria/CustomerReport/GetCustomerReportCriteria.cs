// --------------------------------------------------------------------------
// <copyright file="GetCustomerReportCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.CustomerReport
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Customer  Report Criteria.
    /// </summary>
    public class GetCustomerReportCriteria : CriteriaBase
    {
        private string _SiteIds;

        private string _LanguageIds;

        [SpParam(SqlParamName = "username")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "languageid")]
        public string LanguageIds
        {
            get
            {
                return string.IsNullOrEmpty(_LanguageIds) ? string.Empty : _LanguageIds.Replace("-1", "0");
            }

            set
            {
                _LanguageIds = value;
            }
        }

        [SpParam(SqlParamName = "siteid")]
        public string SiteIds {
            get
            {
                return string.IsNullOrEmpty(_SiteIds) ? string.Empty : _SiteIds.Replace("-1", "0");
            }

            set
            {
                _SiteIds = value;
            }
        }

        [SpParam(SqlParamName = "frombirthday")]
        public DateTime? FromDateBirthDay { get; set; }

        [SpParam(SqlParamName = "tobirthday")]
        public DateTime? ToDateBirthDay { get; set; }

        [SpParam(SqlParamName = "vip")]
        public int VIPTypeId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public string SortField { get; set; }

        [SpParam(Ignored = true)]
        public string SortDirection { get; set; }

        [SpParam(Ignored = true)]
        public bool IsSortReport { get; set; }

        [SpParam(SqlParamName = "totalrows")]
        public int TotalRows { get; set; }

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
