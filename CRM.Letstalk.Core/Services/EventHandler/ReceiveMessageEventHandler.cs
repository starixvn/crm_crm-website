namespace CRM.Letstalk.Core.Services.EventHandler

{
    using System;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.Interfaces;

    public class ReceiveMessageEventHandler : IEventHandler
    {
        private readonly Func<int, ILetstalkService> letstalkService;
        private const string ReplayChat = "Hello, I am Bot.";

        public ReceiveMessageEventHandler(Func<int, ILetstalkService> letstalkService)
        {
            this.letstalkService = letstalkService;
        }

        public async Task EventHandleAsync(HookEvent hookEvent, BotConfig botConfig)
        {
            await letstalkService(botConfig.BotAccountId).PushAsync(hookEvent.Source.UserId, ReplayChat);
        }
    }
}