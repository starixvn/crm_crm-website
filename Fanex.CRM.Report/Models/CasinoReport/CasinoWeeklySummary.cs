// --------------------------------------------------------------------------
// <copyright file="CasinoWeeklySummary.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.22">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CasinoReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Casino Weekly Summary.
    /// </summary>
    [Serializable]
    public class CasinoWeeklySummary
    {
        public string Title { get; set; }

        public IList<string> GameTypes { get; set; }

        public IList<CasinoWeeklyCategory> WeeklyCategories { get; set; }

        public CasinoWeeklyCategory GrandTotal { get; set; }
    }
}
