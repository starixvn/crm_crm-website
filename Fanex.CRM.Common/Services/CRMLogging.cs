using Fanex.Logging;
using Newtonsoft.Json;
using System;

namespace Fanex.CRM.Common.Services
{
    public static class CRMLogging
    {
        public static void Error(Exception exception, object obj)
        {
            if (AppSetting.IsDebugLogging)
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
}
