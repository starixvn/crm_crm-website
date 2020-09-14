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
        private readonly IWriteLogService writeLogService;

        public ChangeGroupNameEventHandler(IBaseService baseService,
            Func<int, ILetstalkService> letstalkService,
            IWriteLogService writeLogService)
        {
            this.baseService = baseService;
            this.letstalkService = letstalkService;
            this.writeLogService = writeLogService;
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
                await writeLogService.WriteErrorLogAsync(new Exception(result.Message));
            }
        }
    }
}