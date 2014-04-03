// --------------------------------------------------------------------------
// <copyright file="GetWagerReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.WagerReport
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetWagerReportCriteria criteria.
    /// </summary>
    public class GetWagerReportCriteria : CriteriaBase
    {
        public int Year { get; set; }

        public override string GetSettingKey()
        {
            return "Report_WagerReport_GetReport";
        }

        public override bool IsValid()
        {
            return Year > 0;
        }
    }
}
