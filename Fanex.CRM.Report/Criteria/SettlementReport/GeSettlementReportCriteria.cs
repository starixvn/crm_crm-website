// --------------------------------------------------------------------------
// <copyright file="GeSettlementReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.SettlementReport
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Settlement Report Criteria.
    /// </summary>
    public class GeSettlementReportCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "year")]
        public int Year { get; set; }

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
