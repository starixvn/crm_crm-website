// --------------------------------------------------------------------------
// <copyright file="CRMStatus.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.11">Created</change>
// </history>

namespace Fanex.CRM.Common.Enumerations
{
    public enum CRMStatus
    {
        /// <summary>
        /// The none status.
        /// </summary>
        None = 0,

        /// <summary>
        /// The pending.
        /// </summary>
        Pending = 11001,

        /// <summary>
        /// The closed.
        /// </summary>
        Closed = 11002,

        /// <summary>
        /// The solved.
        /// </summary>
        Solved = 11003,

        /// <summary>
        /// The deleted.
        /// </summary>
        Deleted = 11004,

        /// <summary>
        /// The checked.
        /// </summary>
        Checked = 1106
    }
}
