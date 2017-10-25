using Fanex.Logging;
using Newtonsoft.Json;
using System;

namespace Fanex.CRM.Common.Services
{
    public static class LoggingHelper
    {
        public static void Error(Exception exception, object obj)
        {
            var info = new[]
            {
                "Object: " + JsonConvert.SerializeObject(obj),
                "ERROR: " + exception.Message,
                "SOURCE: " + exception.Source,
                "TARGETSITE: " + exception.TargetSite,
                "STACKTRACE: " + exception.StackTrace
            };
            LogService.Error(string.Join(Environment.NewLine, info), exception);
        }
    }
}
