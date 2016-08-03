// --------------------------------------------------------------------------
// <copyright file="IPHelper.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Helpers
{
    using System.Web;

    /// <summary>
    /// IP Helper.
    /// </summary>
    public static class IPHelper
    {
        public static string ClientIP
        {
            get
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
        }
    }
}
