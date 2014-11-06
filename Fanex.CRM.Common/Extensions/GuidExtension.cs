// --------------------------------------------------------------------------
// <copyright file="GuidExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Extensions
{
    using System;
    using System.Diagnostics;
    using Fanex.CRM.Common.Helpers;

    /// <summary>
    /// Globally unique identifier Extension.
    /// </summary>
    public static class GuidExtension
    {
        [DebuggerStepThrough]
        public static string Shrink(this Guid target)
        {
            ArgumentChecker.IsNotEmpty(target, "target");
            string base64 = Convert.ToBase64String(target.ToByteArray());
            string encoded = base64.Replace("/", "_").Replace("+", "-");
            return encoded.Substring(0, 22);
        }

        [DebuggerStepThrough]
        public static bool IsEmpty(this Guid target)
        {
            return target == Guid.Empty;
        }
    }
}