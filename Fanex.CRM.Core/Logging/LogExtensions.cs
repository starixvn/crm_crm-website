// --------------------------------------------------------------------------
// <copyright file="LogExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="bobby.nguyen" date="2011.11.01">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Logging
{
    using System;
    using System.IO;
    using System.Web;

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
            string message = ex.Message +
                             "<br>\r\nSOURCE: " + ex.Source +
                             "<br>\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                             "<br>\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                             "<br>\r\nFORM: " + HttpContext.Current.Request.Form.ToString() +
                             "<br>\r\nQUERYSTRING: " + HttpContext.Current.Request.QueryString.ToString() +
                             "<br>\r\nTARGETSITE: " + ex.TargetSite +
                             "<br>\r\nSTACKTRACE: " + ex.StackTrace;
            Fanex.Logging.Logger.Log.Error(message);
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="logContent">Log content.</param>
        public static void WriteLog(string logContent)
        {
            var outPath = string.Format(@"{0}\Logs\Error", HttpContext.Current.Server.MapPath("~/"));

            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            outPath += string.Format(@"\{0:yyyy-MM-dd}.txt", DateTime.Now.Date);

            var fs = new FileStream(outPath, FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);

            sw.WriteLine(DateTime.Now + ": " + logContent);

            sw.Close();
            fs.Close();
        }
    }
}
