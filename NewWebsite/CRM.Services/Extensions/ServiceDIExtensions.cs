namespace CRM.Services.Extensions
{
    using CRM.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceDIExtensions
    {
        public static void ResolveServiceDI(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<INotificationListService, NotificationListService>();
            services.AddTransient<INotificationTemplateService, NotificationTemplateService>();
            services.AddTransient<INotificationTypeService, NotificationTypeService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<INotificationListRepository, NotificationListRepository>();
            services.AddTransient<INotificationTypeRepository, NotificationTypeRepository>();
        }
    }
}