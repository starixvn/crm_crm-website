namespace CRM.Letstalk.Core.Services.EventHandler
{
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Core.Criteria;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.Interfaces;

    public class KickOutBotEventHandler : IEventHandler
    {
        private readonly IBaseService baseService;

        public KickOutBotEventHandler(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task EventHandleAsync(HookEvent hookEvent, BotConfig botConfig)
        {
            var command = new RemoveBotFromGroupCommand
            {
                ConversationGroupId = hookEvent.Source.GroupId,
                BotId = botConfig.BotAccountId,
            };

            await baseService.ExecuteNonQueryAsync(command);
        }
    }
}