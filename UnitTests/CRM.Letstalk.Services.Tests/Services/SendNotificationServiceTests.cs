namespace CRM.Letstalk.Services.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Services.Constants;
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
            configuration = Substitute.For<IConfiguration>();
            func(Arg.Any<int>()).Returns(letstalkService);
            baseService = Substitute.For<IBaseService>();
            logService = Substitute.For<ICRMLogService>();
            service = new SendNotificationService(baseService, logService, configuration, func);
        }

        [Fact]
        public async Task SendNotificationToLetstalk_WhenNoHaveAnyNotification_DoNothing()
        {
            baseService.GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>()).Returns(new List<NotificationMessage>());

            await service.SendNotificationToLetstalk();

            baseService.Received(1);
            logService.DidNotReceive();
            letstalkService.DidNotReceive();
        }

        [Fact]
        public async Task SendNotificationToLetstalk_WhenHaveOnlyBotRemoveNotification_WillUpdateStatusWithBotRemoveReason()
        {
            var notifications = new List<NotificationMessage>
            {
                new NotificationMessage {BotId = 0, ConversationGroupId = 1, NotificationId = 1},
                new NotificationMessage {BotId = 0, ConversationGroupId = 2, NotificationId = 2},
            };
            baseService.GetDataAsync<NotificationMessage>(Arg.Any<GetNotificationToSendCriteria>()).Returns(notifications);

            var results = new List<SendNotificationResult>
            {
                new SendNotificationResult {ConversationGroupId = 1, NotificationId = 1, Status = SendNotificationStatus.Error, ErrMsg = SendNotificationErrMsg.BotRemovedMsg},
                new SendNotificationResult {ConversationGroupId = 2, NotificationId = 2, Status = SendNotificationStatus.Error, ErrMsg = SendNotificationErrMsg.BotRemovedMsg}
            };
            var command = new UpdateNotificationStatusCommand { XmlData = results.ToXML().ToString() };
            await service.SendNotificationToLetstalk();

            await baseService.ExecuteNonQueryAsync(Arg.Is(command));
        }
    }
}