namespace CRM.Letstalk.Core.Tests.Services.EventHandler
{
    using System;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Models.Results;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Core.Criteria;
    using CRM.Letstalk.Core.Models;
    using CRM.Letstalk.Core.Services.EventHandler;
    using NSubstitute;
    using Xunit;

    public class JoinGroupEventHandlerTests
    {
        private readonly JoinGroupEventHandler joinGroupEventHandler;
        private readonly IBaseService baseService;
        private readonly IWriteLogService writeLogService;
        private readonly ILetstalkService letstalkService;

        public JoinGroupEventHandlerTests()
        {
            var func = Substitute.For<Func<int, ILetstalkService>>();
            letstalkService = Substitute.For<ILetstalkService>();
            func(Arg.Any<int>()).Returns(letstalkService);
            baseService = Substitute.For<IBaseService>();
            writeLogService = Substitute.For<IWriteLogService>();

            joinGroupEventHandler = new JoinGroupEventHandler(baseService, func, writeLogService);
        }

        [Fact]
        public async Task EventHandleAsync_WhenCalled_UpdateRoomInfoFailed_WillWritelog()
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

            var roomData = new RoomData
            {
                Title = "ChatGroup 1"
            };

            letstalkService.GetChatRoomData(Arg.Any<string>()).Returns(roomData);

            baseService.ExecuteNonQueryAsync(Arg.Any<AddBotToGroupCommand>()).Returns(new CommandResult { Code = 1, Message = "Error" });

            await joinGroupEventHandler.EventHandleAsync(hookEvent, botConfig);

            await writeLogService.Received().WriteErrorLogAsync(Arg.Any<Exception>());
        }

        [Fact]
        public async Task EventHandleAsync_WhenCalled_UpdateRoomInfoSuccess_DidNotCallWriteLog()
        {
            var hookEvent = new HookEvent
            {
                Source = new EventSource
                {
                    GroupId = "Group1"
                }
            };

            var botConfig = new BotConfig
            {
                BotAccountId = 10000,
                BotUserId = "@ow_local"
            };

            var roomData = new RoomData
            {
                Title = "ChatGroup 1"
            };

            letstalkService.GetChatRoomData(Arg.Any<string>()).Returns(roomData);

            baseService.ExecuteNonQueryAsync(Arg.Any<AddBotToGroupCommand>()).Returns(new CommandResult { Code = 0, Message = "Error" });

            await joinGroupEventHandler.EventHandleAsync(hookEvent, botConfig);

            await writeLogService.DidNotReceive().WriteErrorLogAsync(Arg.Any<Exception>());
        }
    }
}