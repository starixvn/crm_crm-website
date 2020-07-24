namespace CRM.Core.Extensions
{
    using CRM.Core.Repositories;
    using CRM.Core.Services;
    using Fanex.Data.Repository;
    using Microsoft.Extensions.DependencyInjection;

    public static class CoreDIExtensions
    {
        public static void ResolveCoreDI(this IServiceCollection services)
        {
            services.AddTransient<IDynamicRepository, DynamicRepository>();
            services.AddTransient<IBaseRepository, BaseRepository>();
            services.AddTransient<IBaseService, BaseService>();
        }
    }
}