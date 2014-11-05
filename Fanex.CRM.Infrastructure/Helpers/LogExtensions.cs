// --------------------------------------------------------------------------
// <copyright file="LogExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="bobby.nguyen" date="2011.11.01">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Helpers
{
    using System;
    using System.Web;
    using Fanex.Logging;

    /// <summary>
    /// Log Extensions.
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Exceptions the handler.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public static void ExceptionHandler(Exception ex)
        {
            var request = HttpContext.Current.Request;
            string message = ex.Message +
                             "<br>\r\nSOURCE: " + ex.Source +
                             "<br>\r\nDOMAIN: " + request.Headers["Host"] +
                             "<br>\r\nREFERRER: " + request.UrlReferrer +
                             "<br>\r\nQUERYSTRING: " + request.QueryString.ToString() +
                             "<br>\r\nTARGETSITE: " + ex.TargetSite +
                             "<br>\r\nSTACKTRACE: " + ex.StackTrace;
            Logger.Log.Error(message);
        }
    }
}
