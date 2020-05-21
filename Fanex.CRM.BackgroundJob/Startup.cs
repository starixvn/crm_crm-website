namespace Fanex.CRM.BackgroundJob
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Filters;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Repositories;
    using Fanex.CRM.BackgroundJob.Services;
    using Fanex.Data;
    using Fanex.Data.MySql;
    using Fanex.Data.Repository;
    using Fanex.Data.SqlServer;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.AspNetCore;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Sentry;
    using Hangfire;
    using Hangfire.Storage.MySql;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ONEbook.UM.AspNetCore;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using Sentry;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connections = Configuration.GetSection("ConnectionStrings")
                .GetChildren()
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.GetValue<string>("Name")))
                .ToDictionary(x => x.GetValue<string>("Name"),
                    y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName")));
            string spSettings = $"~/App_Data/{Configuration["Environment"]}";

            DbSettingProviderManager
               .StartNewSession()
               .Use(connections)
               .WithMySql()
               .WithSqlServer()
               .UseDefaultDbSettingProvider(spSettings)
               .Run();

            LogManager
                .SetDefaultLogCategory(Configuration.GetSection("Fanex.Logging:LogCategory").Value)
                .Use(new List<ILogEngine>
                {
                    new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(Configuration.GetSection("Fanex.Logging:SentryEngineOptions:Dsn").Value)
                    })
                });

            ExceptionHandler
                .UseConfig(new LoggingConfiguration())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = Configuration.GetSection("AuthSettings:LoginPath").Value;
                options.LogoutPath = Configuration.GetSection("AuthSettings:LogoutPath").Value;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(Configuration["AuthSettings:SessionTimeout"]));
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
                options.Events.OnSigningOut = context =>
                {
                    context.Response.Redirect(options.LoginPath);
                    return Task.CompletedTask;
                };
            });

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(Configuration.GetSection("AuthSettings:SessionTimeout").Value));
            });

            RegisterHangfire(services, connections.FirstOrDefault(x => x.Key.Equals("Hangfire")).Value.ConnectionString);

            services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings").Bind(options));
            services.Configure<HangfireAuthentication>(options => Configuration.GetSection("HangfireSettings:Authentication").Bind(options));
            services.Configure<List<HangfireJobSetting>>(options => Configuration.GetSection("HangfireSettings:JobSettings").Bind(options));

            RegisterDI(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Auth/Error");
            }

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            }).UseHangfireServer(options: new BackgroundJobServerOptions
            {
                WorkerCount = int.Parse(Configuration.GetSection("HangfireSettings:WorkerCount").Value),
                ServerName = Environment.MachineName,
                Queues = new[] { QueuePriority.Critical, QueuePriority.Default }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Auth}/{action=Login}/{id?}");
            });
        }

        private void RegisterDI(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton(new UMClientConfiguration().Initialize(Configuration));
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient, UMRestClient>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();

            services.AddTransient<IRecurringJobManager, RecurringJobManager>();
            services.AddScoped<ILogger, Logger>();
            services.AddScoped<IDynamicRepository, DynamicRepository>();
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<ISystemSettingService, SystemSettingService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IBackgroundService, BackgroundService>();
        }

        private static void RegisterHangfire(IServiceCollection services, string connectionString)
        {
            var connection = Cryptography.DecryptConnectionString(connectionString);
            var options = new MySqlStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(2),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(2),
                TablesPrefix = "CRMHangfire_"
            };
            services.AddHangfire(x => x.UseStorage(new MySqlStorage(connection, options)));
        }
    }
}