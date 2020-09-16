namespace CRM.Letstalk.Services.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Models.Results;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Services.Criteria;
    using CRM.Letstalk.Services.Models.SendNotification;
    using CRM.Letstalk.Services.Services;
    using Microsoft.Extensions.Configuration;
    using NSubstitute;
    using Xunit;

    public class SendNotificationServiceTests
    {
        private readonly SendNotificationService service;
        private readonly IBaseService baseService;
        private readonly IConfiguration configuration;
        private readonly ICRMLogService logService;
        private readonly ILetstalkService letstalkService;

        public SendNotificationServiceTests()
        {
            var func = Substitute.For<Func<int, ILetstalkService>>();
            letstalkService = Substitute.For<ILetstalkService>();
            func(Arg.Any<int>()).Returns(letstalkService);

            configuration = Substitute.For<IConfiguration>();
            baseService = Substitute.For<IBaseService>();
            logService = Substitute.For<ICRMLogService>();
            service = new SendNotificationService(baseService, logService, configuration, func);
        }

        [Fact]
        public async Task SendNotificationToLetstalk_WhenNoHaveAnyNotification_DoNothing()
        {
            baseService.GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>()).Returns(new List<NotificationMessage>());

            await service.SendNotificationToLetstalk();

            await baseService.Received(1).GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>());
            await logService.DidNotReceive().ErrorAsync(Arg.Any<Exception>());
            await letstalkService.DidNotReceive().PushAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task SendNotificationToLetstalk_WhenHaveOnlyBotRemoveNotification_WillUpdateStatusWithBotRemoveReason()
        {
            var notifications = new List<NotificationMessage>
            {
                new NotificationMessage {BotId = 0, ConversationGroupId = 1, NotificationId = 1, ChatTypeId = 1},
                new NotificationMessage {BotId = 0, ConversationGroupId = 2, NotificationId = 2, ChatTypeId = 2}
            };
            baseService.GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>()).Returns(notifications);

            await service.SendNotificationToLetstalk();

            await baseService.Received(1).GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>());
            await baseService.Received(1).ExecuteNonQueryAsync(Arg.Any<UpdateNotificationStatusCommand>());
            await logService.DidNotReceive().ErrorAsync(Arg.Any<Exception>());
        }

        [Fact]
        public async Task SendNotificationToLetstalk_WhenHaveNotification_AndSendFailedAllNotification_WillCallsExpectedNumberLetstalkService()
        {
            var notifications = FakeNotiftcation();
            baseService.GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>()).Returns(notifications);
            await service.SendNotificationToLetstalk();

            await baseService.Received(1).GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>());
            await letstalkService.Received(3).PushAsync(Arg.Any<string>(), Arg.Any<string>());
            await letstalkService.Received(2).PushRoomAsync(Arg.Any<string>(), Arg.Any<string>());
            await baseService.Received(1).ExecuteNonQueryAsync(Arg.Any<UpdateNotificationStatusCommand>());
            await logService.Received(5).ErrorAsync(Arg.Any<Exception>());
        }

        [Fact]
        public async Task SendNotificationToLetstalk_SendFailedOnlyPersonal_WillCallsExpectedNumberLetstalkServiceAndWriteLogService()
        {
            var notifications = FakeNotiftcation();
            baseService.GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>()).Returns(notifications);
            letstalkService.PushRoomAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new BaseResult { Status = 1 });
            await service.SendNotificationToLetstalk();

            await letstalkService.Received(3).PushAsync(Arg.Any<string>(), Arg.Any<string>());
            await letstalkService.Received(2).PushRoomAsync(Arg.Any<string>(), Arg.Any<string>());
            await logService.Received(3).ErrorAsync(Arg.Any<Exception>());
        }

        [Fact]
        public async Task SendNotificationToLetstalk_SendFailedOnlyGroup_WillCallsExpectedNumberLetstalkServiceAndWriteLogService()
        {
            var notifications = FakeNotiftcation();
            baseService.GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>()).Returns(notifications);
            letstalkService.PushAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new PushMessageResult { Status = 1 });
            await service.SendNotificationToLetstalk();

            await logService.Received(2).ErrorAsync(Arg.Any<Exception>());
        }

        [Fact]
        public async Task SendNotificationToLetstalk_SendPersonalWithBlockUser_WillCallsExpectedNumberLetstalkServiceAndWriteLogService()
        {
            var notifications = FakeNotiftcation();
            baseService.GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>()).Returns(notifications);
            letstalkService.PushAsync(Arg.Is("4"), Arg.Any<string>()).Returns(new PushMessageResult { Status = 1, BlockList = new List<string> { "4" } });
            letstalkService.PushAsync(Arg.Is("5"), Arg.Any<string>()).Returns(new PushMessageResult { Status = 1, FailList = new List<string> { "5" } });
            await service.SendNotificationToLetstalk();
            await logService.Received(3).ErrorAsync(Arg.Any<Exception>());
        }

        [Fact]
        public async Task SendNotificationToLetstalk_WhenSendToLetstalk_AlwayWriteLogInLogMessage()
        {
            var notifications = FakeNotiftcation();
            baseService.GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>()).Returns(notifications);
            letstalkService.PushRoomAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new BaseResult { Status = 1 });
            letstalkService.PushAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new PushMessageResult { Status = 1 });

            await service.SendNotificationToLetstalk();

            await baseService.Received(5).ExecuteNonQueryAsync(Arg.Any<WriteLogMessageCommand>());
        }

        private List<NotificationMessage> FakeNotiftcation()
        {
            return new List<NotificationMessage>
            {
                new NotificationMessage {BotId = 100, ConversationGroupId = 1, NotificationId = 1, ChatTypeId = 1, ConversationId = "1"},
                new NotificationMessage {BotId = 100, ConversationGroupId = 2, NotificationId = 2, ChatTypeId = 1, ConversationId = "2"},
                new NotificationMessage {BotId = 100, ConversationGroupId = 3, NotificationId = 3, ChatTypeId = 2, ConversationId = "3"},
                new NotificationMessage {BotId = 100, ConversationGroupId = 4, NotificationId = 4, ChatTypeId = 2, ConversationId = "4"},
                new NotificationMessage {BotId = 100, ConversationGroupId = 5, NotificationId = 5, ChatTypeId = 2, ConversationId = "5"},
            };
        }
    }
}