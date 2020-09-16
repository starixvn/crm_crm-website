namespace CRM.Letstalk.Services.Extentions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CRM.Core.Repositories;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Services.Services;
    using CRM.Letstalk.Services.Services.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class LetstalkServiceDI
    {
        public static void ResolveLetstalkServiceDI(this IServiceCollection services, IConfiguration configuration)
        {
            var botConfigSection = configuration.GetSection("LetstalkBotConfigurations");
            services.Configure<List<BotConfig>>(options => botConfigSection.Bind(options));
            var botConfigs = botConfigSection.Get<List<BotConfig>>();
            services.AddSingleton(botConfigs);
            services.AddTransient<IBaseRepository, BaseRepository>();
            services.AddTransient<IBaseService, BaseService>();
            services.AddTransient<IWriteLogService, WriteLogService>();
            services.AddTransient<ISendNotificationService, SendNotificationService>();
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