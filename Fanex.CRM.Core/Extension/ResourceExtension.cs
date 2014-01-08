// --------------------------------------------------------------------------
// <copyright file="ResourceExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Extension
{
    using System;
    using System.Web;

    /// <summary>
    /// Resource Extension.
    /// </summary>
    public class ResourceExtension
    {
        public enum Resources
        {
            /// <summary>
            /// The error message type.
            /// </summary>
            ErrorMsg,

            /// <summary>
            /// The language type.
            /// </summary>
            Language
        }

        public static string GetResourceValue(Resources resource, string key)
        {
            try
            {
                return HttpContext.GetGlobalResourceObject(resource.ToString(), key).ToString();
            }
            catch (Exception)
            {
                throw new Exception("Key: " + key + " does not exist in resource: " + resource.ToString());
            }
        }
    }
}
