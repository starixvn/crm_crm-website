// --------------------------------------------------------------------------
// <copyright file="ActionLog.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Action Log class.
    /// </summary>
    [Serializable]
    public abstract class ActionLog
    {
        public ActionLog()
        {
            FieldNames = new List<string>(10);
            OldValuesList = new List<string>(10);
            NewValuesList = new List<string>(10);
        }

        public DateTime LogDate { get; set; }

        public string Username { get; set; }

        public string ActionName { get; set; }

        public string OldValues { get; set; }

        public string NewValues { get; set; }

        public IList<string> FieldNames { get; set; }

        public IList<string> OldValuesList { get; set; }

        public IList<string> NewValuesList { get; set; }
    }
}
