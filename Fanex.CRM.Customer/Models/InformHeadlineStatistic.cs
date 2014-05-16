// --------------------------------------------------------------------------
// <copyright file="InformHeadlineStatistic.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.14">Created</change>
// </history>

namespace Fanex.CRM.Customer.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// InformHeadlineStatistic class.
    /// </summary>
    [Serializable]
    public class InformHeadlineStatistic
    {
        public string ReportTitle { get; set; }

        public IList<InformHeadlineStatus> Informs { get; set; }
    }
}
