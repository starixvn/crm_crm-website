namespace CRM.Services.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Repositories;
    using CRM.Services.Interfaces;
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

        public static void ResolveLetstalkServiceDI(this IServiceCollection services, IList<BotConfig> botConfigs)
        {
            services.AddTransient<ICryptographyService, CryptographyService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<RestClientService>();
            services.AddTransient<LetstalkService>();
            services.AddTransient<Func<int, ILetstalkService>>(serviceProvider => botId =>
            {
                var botConfig = botConfigs.FirstOrDefault(config => config.BotAccountId == botId);

                if (botConfig == null)
                {
                    throw new ArgumentNullException("Bot not found", new Exception($"The bot ID: {botId} does not existed in configuration"));
                }

                var restClientService = new RestClientService(
                RestClientService.CreateDefaultClient(botConfig.ApiEndpoint, botConfig.ProxyAddress, botConfig.ByPassProxyLocal, botConfig.IsUseProxy),
                new JwtService(),
                botConfig);

                return new LetstalkService(serviceProvider.GetRequiredService<ICryptographyService>(), restClientService, botConfig);
            });
        }
    }
}