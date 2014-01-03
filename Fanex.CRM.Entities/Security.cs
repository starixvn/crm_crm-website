// --------------------------------------------------------------------------
// <copyright file="Security.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    /// <summary>
    /// Security information.
    /// </summary>
    public class Security
    {
        public string FileNames { get; set; }

        public string FuncName { get; set; }

        public int ActualPolicy { get; set; }
    }
}
