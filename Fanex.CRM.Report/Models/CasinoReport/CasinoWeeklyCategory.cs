// --------------------------------------------------------------------------
// <copyright file="CasinoWeeklyCategory.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.22">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CasinoReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Casino Weekly Category.
    /// </summary>
    [Serializable]
    public class CasinoWeeklyCategory
    {
        public string Name { get; set; }

        public IList<int> GameTypeValues { get; set; }

        public int GrandTotal
        {
            get
            {
                return GameTypeValues == null ? 0 : GameTypeValues.Sum();
            }
        }
    }
}
