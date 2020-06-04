namespace Fanex.CRM.BackgroundJob
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.ServiceProcess;
    using Fanex.CRM.BackgroundJob.Services;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());
            var host = builder.Build();
            host.RunAsCustomService(isService);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseEnvironment(configuration["Environment"] ?? string.Empty)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
                .UseUrls(configuration["WebHostUrls"]);
        }

        private static void RunAsCustomService(this IWebHost host, bool isService)
        {
            var webHostService = new CustomWebHostService(host);
            if (isService)
            {
                ServiceBase.Run(webHostService);
            }
            else
            {
                webHostService.RunHangfireJobs();
                host.Run();
            }
        }
    }
}