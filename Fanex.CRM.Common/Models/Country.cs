// --------------------------------------------------------------------------
// <copyright file="CountryInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Models
{
    using System;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Country Information.
    /// </summary>
    [Serializable]
    public class Country : BaseModel
    {
        public int CountryCode { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
}
