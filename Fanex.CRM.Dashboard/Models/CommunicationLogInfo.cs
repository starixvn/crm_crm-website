// --------------------------------------------------------------------------
// <copyright file="CommunicationLogInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Models
{
    using System;
    using Fanex.CRM.Common.Models;

    /// <summary>
    /// Communication Log Information.
    /// </summary>
    [Serializable]
    public class CommunicationLogInfo : BaseCommunicationLog
    {
        public new DateTime StartTime { get; set; }

        public new DateTime EndTime { get; set; }
    }
}
