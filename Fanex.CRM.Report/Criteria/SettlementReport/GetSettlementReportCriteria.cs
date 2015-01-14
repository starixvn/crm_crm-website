// --------------------------------------------------------------------------
// <copyright file="GetSettlementReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.SettlementReport
{
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Settlement Report Criteria.
    /// </summary>
    public class GetSettlementReportCriteria : CriteriaBase
    {
        public int Year { get; set; }

        public int TimeSpan
        {
            get
            {
                return AppSetting.TimeSpan * -1;
            }
        }        

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGetFromCached { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGetFullReport { get; set; }

        public override string GetSettingKey()
        {
            return "Report_SettlementReport_GetReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
