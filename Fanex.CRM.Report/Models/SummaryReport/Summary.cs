// --------------------------------------------------------------------------
// <copyright file="Summary.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.SummaryReport
{
    using System;

    /// <summary>
    /// SummaryModel model.
    /// </summary>
    [Serializable]
    public class Summary
    {
        public int Id { get; set; }

        public string Month { get; set; }

        public int Amount { get; set; }
    }
}
