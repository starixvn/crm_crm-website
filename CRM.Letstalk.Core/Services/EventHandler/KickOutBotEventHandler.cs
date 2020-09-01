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
        private readonly IWriteLogService writeLogService;

        public KickOutBotEventHandler(IBaseService baseService, IWriteLogService writeLogService)
        {
            this.baseService = baseService;
            this.writeLogService = writeLogService;
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
                await writeLogService.WriteErrorLogAsync(new Exception(result.Message));
            }
        }
    }
}