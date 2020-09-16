namespace CRM.Letstalk.Core.Services.EventHandler
{
    using System;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Criteria;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.Interfaces;

    public class JoinGroupEventHandler : IEventHandler
    {
        private readonly IBaseService baseService;
        private readonly Func<int, ILetstalkService> letstalkService;
        private readonly ICRMLogService logService;

        public JoinGroupEventHandler(IBaseService baseService, Func<int,
            ILetstalkService> letstalkService,
            ICRMLogService logService)
        {
            this.baseService = baseService;
            this.letstalkService = letstalkService;
            this.logService = logService;
        }

        public async Task EventHandleAsync(HookEvent hookEvent, BotConfig botConfig)
        {
            var displayName = await GetChatRoomDisplayName(botConfig, hookEvent.Source.GroupId);

            var command = new AddBotToGroupCommand
            {
                GroupName = displayName,
                ConversationId = hookEvent.Source.GroupId,
                BotId = botConfig.BotAccountId,
                BotName = botConfig.BotUserId
            };

            var result = await baseService.ExecuteNonQueryAsync(command);

            if (!result.IsSuccessful)
            {
                await logService.ErrorAsync(new Exception(result.Message));
            }
        }

        private async Task<string> GetChatRoomDisplayName(BotConfig botConfig, string groupId)
        {
            var roomData = await letstalkService(botConfig.BotAccountId).GetChatRoomData(groupId);

            return roomData.Title;
        }
    }
}