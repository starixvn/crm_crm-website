// --------------------------------------------------------------------------
// <copyright file="GetCasinoDetailCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get Casino Daily Detail Criteria.
    /// </summary>
    public class GetCasinoDetailCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; }

        [SpParam(Ignored = true)]
        public bool FullReport { get; set; }

        [SpParam(Ignored = true)]
        public int ReportType { get; set; }

        public override string GetSettingKey()
        {
            return "casinodailydetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
