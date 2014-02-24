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

    /// <summary>
    /// Setting Log model.
    /// </summary>
    [Serializable]
    public class SettingLog : ActionLog
    {
        public int Id { get; set; }

        public string Entity { get; set; }
    }
}
