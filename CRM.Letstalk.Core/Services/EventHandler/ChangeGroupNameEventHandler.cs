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

    public class ChangeGroupNameEventHandler : IEventHandler
    {
        private readonly IBaseService baseService;
        private readonly Func<int, ILetstalkService> letstalkService;

        public ChangeGroupNameEventHandler(IBaseService baseService,
            Func<int, ILetstalkService> letstalkService)
        {
            this.baseService = baseService;
            this.letstalkService = letstalkService;
        }

        public async Task EventHandleAsync(HookEvent hookEvent, BotConfig botConfig)
        {
            var roomData = await letstalkService(botConfig.BotAccountId).GetChatRoomData(hookEvent.Source.GroupId);
            var command = new UpdateGroupNameCommand
            {
                ConversationGroupId = hookEvent.Source.GroupId,
                DisplayName = roomData.Title
            };

            await baseService.ExecuteNonQueryAsync(command);
        }
    }
}