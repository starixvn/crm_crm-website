namespace CRM.Letstalk.Core.Services.EventHandler
{
    using System;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Core.Criteria;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.Interfaces;

    public class KickOutBotEventHandler : IEventHandler
    {
        private readonly IBaseService baseService;
        private readonly ICRMLogService logService;

        public KickOutBotEventHandler(IBaseService baseService, ICRMLogService logService)
        {
            this.baseService = baseService;
            this.logService = logService;
        }

        public async Task EventHandleAsync(HookEvent hookEvent, BotConfig botConfig)
        {
            var command = new RemoveBotFromGroupCommand
            {
                ConversationId = hookEvent.Source.GroupId,
                BotId = botConfig.BotAccountId,
            };

            var result = await baseService.ExecuteNonQueryAsync(command);

            if (!result.IsSuccessful)
            {
                await logService.ErrorAsync(new Exception(result.Message));
            }
        }
    }
}