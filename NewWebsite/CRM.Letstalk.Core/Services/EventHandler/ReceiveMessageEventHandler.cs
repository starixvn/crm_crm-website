namespace CRM.Letstalk.Core.Services.EventHandler

{
    using System;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.Interfaces;
    using Newtonsoft.Json;

    public class ReceiveMessageEventHandler : IEventHandler
    {
        private readonly Func<int, ILetstalkService> letstalkService;
        private readonly ICRMLogService cRMLogService;
        private const string ReplayChat = "Hello, I am Bot.";

        public ReceiveMessageEventHandler(Func<int, ILetstalkService> letstalkService, ICRMLogService cRMLogService)
        {
            this.letstalkService = letstalkService;
            this.cRMLogService = cRMLogService;
        }

        public async Task EventHandleAsync(HookEvent hookEvent, BotConfig botConfig)
        {
            cRMLogService.LogInformation(JsonConvert.SerializeObject(hookEvent));
            cRMLogService.LogInformation(JsonConvert.SerializeObject(botConfig));
            await letstalkService(botConfig.BotAccountId).PushAsync(hookEvent.Source.UserId, ReplayChat);
        }
    }
}