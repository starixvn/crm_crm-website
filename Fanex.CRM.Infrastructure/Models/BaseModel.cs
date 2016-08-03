// --------------------------------------------------------------------------
// <copyright file="BaseModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Models
{
    using System;

    /// <summary>
    /// Base Model.
    /// </summary>
    [Serializable]
    public class BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
