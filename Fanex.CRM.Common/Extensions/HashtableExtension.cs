// --------------------------------------------------------------------------
// <copyright file="StringExtension.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Extensions
{
    using System.Collections;
    using System.Diagnostics;

    /// <summary>
    /// String Extension.
    /// </summary>
    public static class HashtableExtension
    {
        [DebuggerStepThrough]
        public static void AddIf(this Hashtable target, bool condition, string key, object value)
        {
            if (condition)
            {
                target.Add(key, value);
            }
        }
    }
}