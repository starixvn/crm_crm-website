namespace CRM.Letstalk.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Extensions;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class WebhookReceiver : IWebhookReceiver
    {
        private readonly IJwtService jwtService;
        private readonly ICRMLogService logService;
        private readonly Func<string, IEventHandler> eventHandler;
        private readonly List<BotConfig> botConfigs;

        public WebhookReceiver(IJwtService jwtService,
            ICRMLogService logService,
            Func<string, IEventHandler> eventHandler,
            List<BotConfig> botConfigs)
        {
            this.jwtService = jwtService;
            this.logService = logService;
            this.eventHandler = eventHandler;
            this.botConfigs = botConfigs;
        }

        public async Task ReceiveAsync(HttpRequest request, int botId)
        {
            logService.LogInformation("----------------Receive Async------------------");
            CheckArgumentHelper.CheckValidBot(botConfigs, botId);

            string requestJson;
            var botConfig = botConfigs.FirstOrDefault(config => config.BotAccountId == botId);
            using (TextReader reader = new StreamReader(request.Body))
            {
                requestJson = await reader.ReadToEndAsync();
                logService.LogInformation(requestJson);
                logService.LogInformation(JsonConvert.SerializeObject(botConfig));
            }

            var rawData = jwtService.Decode(botConfig.SecureKey, requestJson.Trim());
            var data = JObject.Parse(rawData);
            var hookEvent = data.ToObject<HookEvent>();
            logService.LogInformation(JsonConvert.SerializeObject(hookEvent));
            logService.LogInformation("----------------End Receive Async------------------");
            await eventHandler(hookEvent.Type).EventHandleAsync(hookEvent, botConfig);
        }
    }
}