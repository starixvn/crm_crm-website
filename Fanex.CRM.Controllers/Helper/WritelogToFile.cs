namespace Fanex.CRM.Controllers.Helper
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    public static class WritelogToFile
    {
        public static void Writelog(object content, string title = "")
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log.txt");
            if (!File.Exists(path))
            {
                File.CreateText(path);
            }
            using (StreamWriter writer = File.AppendText(path))
            {
                Log(content, writer, title);
            }
        }

        private static void Log(object content, TextWriter writer, string title)
        {
            writer.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()} :");
            writer.WriteLine(title);
            writer.WriteLine(JsonConvert.SerializeObject(content));
            writer.WriteLine("-------------------------------");
        }
    }
}