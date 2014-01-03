
namespace Fanex.CRM.Infrastructure.Logging
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Web;
    using System.Threading.Tasks;

    /**
     * public class Log Extensions
     * @Author: bobby.nguyen
     * @Date: 11012011
     **/
    public static class LogExtensions
    {        
        public static void ExceptionHandler(Exception ex)
        {
            Task.Factory.StartNew(() =>
            {
                Fanex.Logging.Logger.Log.Error(ex.Message, ex);
            });
        }
        public static void WriteLog(string sContent)
        {
            var outPath = string.Format(@"{0}\Logs\Error", HttpContext.Current.Server.MapPath("~/"));

            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }

            outPath += string.Format(@"\{0:yyyy-MM-dd}.txt", DateTime.Now.Date);

            var fs = new FileStream(outPath, FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);

            sw.WriteLine(DateTime.Now + ": " + sContent);

            sw.Close();
            fs.Close();
        }
    }
}
