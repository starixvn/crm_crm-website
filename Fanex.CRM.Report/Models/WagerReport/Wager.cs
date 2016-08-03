// --------------------------------------------------------------------------
// <copyright file="Wager.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.WagerReport
{
    using System;

    /// <summary>
    /// Wager model.
    /// </summary>
    [Serializable]
    public class Wager
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }
    }
}
