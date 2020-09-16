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
        private readonly ICRMLogService logService;

        public ChangeGroupNameEventHandler(IBaseService baseService,
            Func<int, ILetstalkService> letstalkService,
            ICRMLogService logService)
        {
            this.baseService = baseService;
            this.letstalkService = letstalkService;
            this.logService = logService;
        }

        public async Task EventHandleAsync(HookEvent hookEvent, BotConfig botConfig)
        {
            var roomData = await letstalkService(botConfig.BotAccountId).GetChatRoomData(hookEvent.Source.GroupId);

            var command = new UpdateGroupNameCommand
            {
                ConversationId = hookEvent.Source.GroupId,
                DisplayName = roomData.Title
            };

            var result = await baseService.ExecuteNonQueryAsync(command);

            if (!result.IsSuccessful)
            {
                await logService.ErrorAsync(new Exception(result.Message));
            }
        }
    }
}