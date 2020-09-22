namespace CRM.BackgroundJob.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Letstalk.Services.Services.Interfaces;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Models.ChatBot;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using Xunit;

    public class ChatBotJobServiceTests
    {
        private ICRMLogService logService;
        private ISystemSettingService systemSettingService;
        private IChatBotTicketVoidingService ticketVoidingService;
        private ISendNotificationService sendNotificationService;
        private readonly IFixture fixture = new Fixture();

        [Fact]
        public async Task PullMessageAndSendNotificationAsync_ServiceIsShutdown_ExpectedWriteLog()
        {
            // Arrange
            var setting = new AbnormalBetSystemSetting { ServiceFlag = false };
            var service = GetService();
            systemSettingService.GetChatBotSystemSettingAsync().Returns(setting);

            // Act
            await service.PullMessageAndSendNotificationAsync();

            // Assert
            logService.Received().LogInformation("Chat Bot Service is shutdown.");
        }

        [Fact]
        public async Task PullMessageAndSendNotificationAsync_TicketInfosAreEmpty_ExpectedCallEditVoidingMessageAsync()
        {
            // Arrange
            var setting = new AbnormalBetSystemSetting { ServiceFlag = true };
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var service = GetService();
            systemSettingService.GetChatBotSystemSettingAsync().Returns(setting);
            ticketVoidingService.GetVoidingMessageAsync().Returns(voidingMessages);
            ticketVoidingService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize).Returns(voidingMessages);
            ticketVoidingService.GetTicketVoidingInfoAsync(voidingMessages).Returns(new List<ChatBotVoidingTicketInfo>());

            // Act
            await service.PullMessageAndSendNotificationAsync();

            // Assert
            await ticketVoidingService.Received().EditVoidingMessageAsync(voidingMessages);
        }

        [Fact]
        public async Task PullMessageAndSendNotificationAsync_ImportTicketInfosUnsuccessful_ExpectedWriteLog()
        {
            // Arrange
            var setting = new AbnormalBetSystemSetting { ServiceFlag = true };
            var messages = fixture.Create<IEnumerable<Message>>();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var ticketInfos = fixture.Create<IEnumerable<ChatBotVoidingTicketInfo>>();
            var commandResult = new CommandResult { Code = 1 };
            var service = GetService();
            systemSettingService.GetChatBotSystemSettingAsync().Returns(setting);
            ticketVoidingService.GetVoidingMessageAsync().Returns(voidingMessages);
            ticketVoidingService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize).Returns(voidingMessages);
            ticketVoidingService.GetMessageInfoAsync(voidingMessages).Returns(messages);
            ticketVoidingService.GetTicketVoidingInfoAsync(voidingMessages).Returns(ticketInfos);
            ticketVoidingService.ImportTicketVoidingAsync(messages, ticketInfos).Returns(commandResult);

            // Act
            await service.PullMessageAndSendNotificationAsync();

            // Assert
            logService.Received().LogInformation(commandResult.Message);
        }

        [Fact]
        public async Task PullMessageAndSendNotificationAsync_ImportTicketInfosSuccessful_ExpectedCallEditVoidingMessageAsync()
        {
            // Arrange
            var setting = new AbnormalBetSystemSetting { ServiceFlag = true };
            var messages = fixture.Create<IEnumerable<Message>>();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var ticketInfos = fixture.Create<IEnumerable<ChatBotVoidingTicketInfo>>();
            var commandResult = new CommandResult { Code = 0 };
            var service = GetService();
            systemSettingService.GetChatBotSystemSettingAsync().Returns(setting);
            ticketVoidingService.GetVoidingMessageAsync().Returns(voidingMessages);
            ticketVoidingService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize).Returns(voidingMessages);
            ticketVoidingService.GetMessageInfoAsync(voidingMessages).Returns(messages);
            ticketVoidingService.GetTicketVoidingInfoAsync(voidingMessages).Returns(ticketInfos);
            ticketVoidingService.ImportTicketVoidingAsync(messages, ticketInfos).Returns(commandResult);

            // Act
            await service.PullMessageAndSendNotificationAsync();

            // Assert
            await ticketVoidingService.Received().EditVoidingMessageAsync(voidingMessages);
        }

        #region Helpers

        private ChatBotJobService GetService()
        {
            logService = Substitute.For<ICRMLogService>();
            systemSettingService = Substitute.For<ISystemSettingService>();
            ticketVoidingService = Substitute.For<IChatBotTicketVoidingService>();
            sendNotificationService = Substitute.For<ISendNotificationService>();
            return new ChatBotJobService(logService, systemSettingService, ticketVoidingService, sendNotificationService);
        }

        #endregion Helpers
    }
}