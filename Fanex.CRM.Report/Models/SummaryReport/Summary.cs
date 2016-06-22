// --------------------------------------------------------------------------
// <copyright file="Summary.cs" company="">
// Copyright (c) . All rights reserved.
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
        public string Month { get; set; }

        public int Amount { get; set; }
    }
}
