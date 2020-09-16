namespace CRM.Core.Extensions
{
    using CRM.Core.Repositories;
    using CRM.Core.Services;
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Microsoft.Extensions.DependencyInjection;

    public static class CoreDIExtensions
    {
        public static void ResolveCoreDI(this IServiceCollection services)
        {
            services.AddSingleton<IDynamicRepository, DynamicRepository>();
            services.AddSingleton<IBaseRepository, BaseRepository>();
            services.AddSingleton<IBaseService, BaseService>();
            services.AddScoped<ILogger, Logger>();
            services.AddScoped<ICRMLogService, CRMLogService>();
        }
    }
}