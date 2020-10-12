namespace CRM.Letstalk.Webhook
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 var env = hostingContext.HostingEnvironment;
                 config.SetBasePath(env.ContentRootPath);
                 config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                 config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                 config.AddEnvironmentVariables();
             })
            .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
            .UseStartup<Startup>();
    }
}