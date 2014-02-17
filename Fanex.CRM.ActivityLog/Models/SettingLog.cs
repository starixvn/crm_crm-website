// --------------------------------------------------------------------------
// <copyright file="SettingLog.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Setting Log model.
    /// </summary>
    public class SettingLog : ActionLog
    {
        public int ID { get; set; }

        public string Entity { get; set; }
    }
}
