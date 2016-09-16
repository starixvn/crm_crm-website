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
        private string _siteIds;

        private string _languageIds;

        private string _positionIds;

        [SpParam(SqlParamName = "username")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "languageid")]
        public string LanguageIds
        {
            get
            {
                return string.IsNullOrEmpty(_languageIds) ? string.Empty : _languageIds.Replace("-1", "0");
            }

            set
            {
                _languageIds = value;
            }
        }

        [SpParam(SqlParamName = "siteid")]
        public string SiteIds
        {
            get
            {
                return string.IsNullOrEmpty(_siteIds) ? string.Empty : _siteIds.Replace("-1", "0");
            }

            set
            {
                _siteIds = value;
            }
        }

        [SpParam(SqlParamName = "vip")]
        public int VIPTypeId { get; set; }

        public string Headline { get; set; }

        [SpParam(SqlParamName = "roleid")]
        public string PositionIds
        {
            get
            {
                return string.IsNullOrEmpty(_positionIds) ? string.Empty : _positionIds.Replace("-1", "0");
            }

            set
            {
                _positionIds = value;
            }
        }

        [SpParam(SqlParamName = "excludeignoredacc")]
        public bool IsExcludeIgnoredAccounts { get; set; }

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
            return "Acc_Crm_Rpt_CustomerReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}