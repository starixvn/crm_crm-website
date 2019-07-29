namespace Fanex.CRM.Common.Extensions
{
    using System;
    using System.Web;
    using Fanex.Logging;

    public static class LogExtensions
    {
        public static void ExceptionHandler(Exception ex)
        {
            var request = HttpContext.Current.Request;
            const string messageTemplate = "{0}<br>\r\nSOURCE: {1}<br>\r\nDOMAIN: {2}<br>\r\nREFERRER: {3}<br>\r\nQUERYSTRING: {4}<br>\r\nTARGETSITE: {5}<br>\r\nSTACKTRACE: {6}";
            Logger.Log.Error(string.Format(
                messageTemplate,
                ex.Message,
                ex.Source,
                request.Headers["Host"],
                request.UrlReferrer,
                request.QueryString,
                ex.TargetSite,
                ex.StackTrace));
        }
    }
}