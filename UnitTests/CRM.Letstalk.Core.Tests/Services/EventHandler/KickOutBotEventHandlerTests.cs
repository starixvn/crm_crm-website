namespace CRM.Letstalk.Core.Tests.Services.EventHandler
{
    using System;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Core.Criteria;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.EventHandler;
    using NSubstitute;
    using Xunit;

    public class KickOutBotEventHandlerTests
    {
        private readonly KickOutBotEventHandler kickOutBotEventHandler;
        private readonly IBaseService baseService;
        private readonly IWriteLogService writeLogService;

        public KickOutBotEventHandlerTests()
        {
            baseService = Substitute.For<IBaseService>();
            writeLogService = Substitute.For<IWriteLogService>();

            kickOutBotEventHandler = new KickOutBotEventHandler(baseService, writeLogService);
        }

        [Fact]
        public async Task EventHandleAsync_WhenCalled_UpdateRoomInfoFailed_WillWritelog()
        {
            var hookEvent = new HookEvent
            {
                Source = new EventSource
                {
                    GroupId = "letstalk"
                }
            };

            var botConfig = new BotConfig
            {
                BotAccountId = 10000,
            };

            baseService.ExecuteNonQueryAsync(Arg.Any<RemoveBotFromGroupCommand>()).Returns(new CommandResult { Code = 1, Message = "Error" });

            await kickOutBotEventHandler.EventHandleAsync(hookEvent, botConfig);

            await writeLogService.Received().WriteErrorLogAsync(Arg.Any<Exception>());
        }

        [Fact]
        public async Task EventHandleAsync_WhenCalled_UpdateRoomInfoSuccess_DidNotCallWriteLog()
        {
            var hookEvent = new HookEvent
            {
                Source = new EventSource
                {
                    UserId = "letstalk"
                }
            };

            var botConfig = new BotConfig
            {
                BotAccountId = 10000,
                BotUserId = "@ow_local"
            };

            baseService.ExecuteNonQueryAsync(Arg.Any<RemoveBotFromGroupCommand>()).Returns(new CommandResult { Code = 0, Message = "Error" });

            await kickOutBotEventHandler.EventHandleAsync(hookEvent, botConfig);

            await writeLogService.DidNotReceive().WriteErrorLogAsync(Arg.Any<Exception>());
        }
    }
}