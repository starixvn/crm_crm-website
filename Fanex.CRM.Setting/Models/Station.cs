// --------------------------------------------------------------------------
// <copyright file="Station.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Setting.Models
{
    using System;

    /// <summary>
    /// Station model.
    /// </summary>
    [Serializable]
    public class Station
    {
        public int StationId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
