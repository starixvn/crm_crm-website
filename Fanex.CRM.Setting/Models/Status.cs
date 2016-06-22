// --------------------------------------------------------------------------
// <copyright file="Status.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Setting.Models
{
    using System;

    /// <summary>
    /// Status model.
    /// </summary>
    [Serializable]
    public class Status
    {
        public int StatusId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
