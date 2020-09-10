namespace CRM.Letstalk.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Constants;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services;
    using CRM.Letstalk.Core.Services.EventHandler;
    using CRM.Letstalk.Core.Services.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RestSharp;

    public static class DIResolver
    {
        public static void ResolveWebhookServices(this IServiceCollection services, IConfiguration configuration)
        {
            var webhookConfigurationSection = configuration.GetSection("WebhookConfigurations");
            services.Configure<List<WebhookConfiguration>>(options => webhookConfigurationSection.Bind(options));
            var webhookConfigs = webhookConfigurationSection.Get<List<WebhookConfiguration>>();
            services.AddSingleton(webhookConfigs);

            services.AddTransient<IWebhookReceiver, WebhookReceiver>();
            services.AddTransient<ICryptographyService, CryptographyService>();
            services.AddTransient<IJwtService, JwtService>();

            services.AddTransient<JoinGroupEventHandler>();
            services.AddTransient<KickOutBotEventHandler>();
            services.AddTransient<ChangeGroupNameEventHandler>();
            services.AddTransient<AddContactEventHandler>();
            services.AddTransient<ReceiveMessageEventHandler>();
            services.AddTransient<Func<string, IEventHandler>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case EventType.JoinGroup:
                        return serviceProvider.GetRequiredService<JoinGroupEventHandler>();

                    case EventType.KickOutGroup:
                        return serviceProvider.GetRequiredService<KickOutBotEventHandler>();

                    case EventType.ChangeGroupName:
                        return serviceProvider.GetRequiredService<ChangeGroupNameEventHandler>();

                    case EventType.Message:
                        return serviceProvider.GetRequiredService<ReceiveMessageEventHandler>();

                    case EventType.AddContact:
                        return serviceProvider.GetRequiredService<AddContactEventHandler>();

                    default:
                        throw new KeyNotFoundException($"The event type {key} have not implements!");
                }
            });
        }

        public static void ResolveLetstalkServices(this IServiceCollection services, IConfiguration configuration)
        {
            var botConfigSections = configuration.GetSection("LetstalkBotConfigurations");
            services.Configure<List<BotConfig>>(options => botConfigSections.Bind(options));
            var botConfigs = botConfigSections.Get<List<BotConfig>>();
            services.AddSingleton(botConfigs);
            services.AddTransient<IRestClient, RestClient>();

            services.AddTransient<RestClientService>();
            services.AddTransient<LetstalkService>();
            services.AddTransient<Func<int, ILetstalkService>>(serviceProvider => botId =>
            {
                CheckArgumentHelper.CheckValidBot(botConfigs, botId);

                var botConfig = botConfigs.FirstOrDefault(config => config.BotAccountId == botId);
                var restClientService = new RestClientService(
                RestClientService.CreateDefaultClient(botConfig.ApiEndpoint, botConfig.ProxyAddress, botConfig.ByPassProxyLocal, botConfig.IsUseProxy),
                new JwtService(),
                botConfig);

                return new LetstalkService(serviceProvider.GetRequiredService<ICryptographyService>(), restClientService, botConfig);
            });
        }
    }
}