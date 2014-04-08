// --------------------------------------------------------------------------
// <copyright file="WagerByMonth.cs" company="Nexcel Solutions Vietnam">
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
    /// WagerByMonth model.
    /// </summary>
    [Serializable]
    public class WagerByMonth
    {
        public string Month { get; set; }

        public IList<Wager> Wagers { get; set; }

        public int Total { get; set; }
    }
}
