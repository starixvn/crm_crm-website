// --------------------------------------------------------------------------
// <copyright file="TimeInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Models
{
    using System;

    /// <summary>
    /// Time Info.
    /// </summary>
    [Serializable]
    public class TimeInfo
    {
        public int Days { get; set; }

        public int Hours { get; set; }

        public int Minutes { get; set; }

        public int Seconds { get; set; }
    }
}
