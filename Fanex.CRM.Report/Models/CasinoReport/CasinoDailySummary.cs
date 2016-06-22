// --------------------------------------------------------------------------
// <copyright file="CasinoDailySummary.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.01">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CasinoReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Casino Daily Summary.
    /// </summary>
    [Serializable]
    public class CasinoDailySummary
    {
        public CasinoDailySummary()
        {
            CategoryCasinoGroups = new List<CategoryCasinoGroup>();
        }

        public string Title { get; set; }

        public IList<CategoryCasinoGroup> CategoryCasinoGroups { get; set; }

        public CategoryCasinoGroup Total { get; set; }
    }
}
