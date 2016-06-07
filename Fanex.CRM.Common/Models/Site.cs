// --------------------------------------------------------------------------
// <copyright file="Site.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Common.Models
{
    using System;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Setting Information.
    /// </summary>
    [Serializable]
    public class Site : BaseModel
    {
        public string AliasName { get; set; }
    }
}
