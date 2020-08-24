namespace CRM.Services.Extensions
{
    using System;
    using CRM.Repositories;
    using CRM.Services.Interfaces;
    using Fanex.Letstalk.Client.Configurations;
    using Fanex.Letstalk.Client.Services;
    using Fanex.Letstalk.Client.Services.Interfaces;
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
            services.AddTransient<IChatTypeMappingService, ChatTypeMappingService>();
        }

        public static void ResolveLetstalkBotConfigDI(this IServiceCollection services)
        {
            services.AddTransient<LetstalkClientService>();

            services.AddTransient<Func<int, ILetstalkClientService>>(serviceProvider => botId =>
            {
                return new LetstalkClientService(LetstalkBotConfigurations.Settings, botId);
            });
        }
    }
}