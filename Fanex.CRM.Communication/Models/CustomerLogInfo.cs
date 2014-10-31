// --------------------------------------------------------------------------
// <copyright file="CustomerLogInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;

    /// <summary>
    /// Log Information.
    /// </summary>
    [Serializable]
    public class CustomerLogInfo
    {
        public int CustomerId { get; set; }

        public List<BaseCommunicationLogInfo> Logs { get; set; }
    }
}
