// --------------------------------------------------------------------------
// <copyright file="WagerReportModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.WagerReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// WagerReport model.
    /// </summary>
    [Serializable]
    public class WagerReportModel
    {
        public string ReportTitle { get; set; }

        public IList<WagerByMonth> WagerByMonths { get; set; }

        public WagerByMonth GrandTotal { get; set; }
    }
}
