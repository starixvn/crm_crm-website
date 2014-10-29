// --------------------------------------------------------------------------
// <copyright file="GetCasinoSummaryCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.30">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.CasinoReport
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Casino Daily Summary Criteria.
    /// </summary>
    public class GetCasinoSummaryCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "type")]
        public int ReportType { get; set; }

        [SpParam(Ignored = true)]
        public bool FromCache { get; set; }

        public override string GetSettingKey()
        {
            return "casinodailysummary";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
