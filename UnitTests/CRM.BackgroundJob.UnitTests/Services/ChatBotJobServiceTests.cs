namespace CRM.BackgroundJob.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Models.ChatBot;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using Xunit;

    public class ChatBotJobServiceTests
    {
        private ICRMLogService logService;
        private IChatBotTicketVoidingService ticketVoidingService;
        private readonly IFixture fixture = new Fixture();

        [Fact]
        public async Task ExecuteJobAsync_TicketInfosAreEmpty_ExpectedCallEditVoidingMessageAsync()
        {
            // Arrange
            var setting = fixture.Create<AbnormalBetSystemSetting>();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var service = GetService();
            ticketVoidingService.GetVoidingMessageAsync().Returns(voidingMessages);
            ticketVoidingService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize).Returns(voidingMessages);
            ticketVoidingService.GetTicketVoidingInfoAsync(voidingMessages).Returns(new List<ChatBotVoidingTicketInfo>());

            // Act
            await service.PullMessageAsync(setting);

            // Assert
            await ticketVoidingService.Received().EditVoidingMessageAsync(voidingMessages);
        }

        [Fact]
        public async Task ExecuteJobAsync_ImportTicketInfosUnsuccessful_ExpectedWriteLog()
        {
            // Arrange
            var setting = fixture.Create<AbnormalBetSystemSetting>();
            var messages = fixture.Create<IEnumerable<Message>>();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var ticketInfos = fixture.Create<IEnumerable<ChatBotVoidingTicketInfo>>();
            var commandResult = new CommandResult { Code = 1 };
            var service = GetService();
            ticketVoidingService.GetVoidingMessageAsync().Returns(voidingMessages);
            ticketVoidingService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize).Returns(voidingMessages);
            ticketVoidingService.GetMessageInfoAsync(voidingMessages).Returns(messages);
            ticketVoidingService.GetTicketVoidingInfoAsync(voidingMessages).Returns(ticketInfos);
            ticketVoidingService.ImportTicketVoidingAsync(messages, ticketInfos).Returns(commandResult);

            // Act
            await service.PullMessageAsync(setting);

            // Assert
            logService.Received().LogInformation(commandResult.Message);
        }

        [Fact]
        public async Task ExecuteJobAsync_ImportTicketInfosSuccessful_ExpectedCallEditVoidingMessageAsync()
        {
            // Arrange
            var setting = fixture.Create<AbnormalBetSystemSetting>();
            var messages = fixture.Create<IEnumerable<Message>>();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var ticketInfos = fixture.Create<IEnumerable<ChatBotVoidingTicketInfo>>();
            var commandResult = new CommandResult { Code = 0 };
            var service = GetService();
            ticketVoidingService.GetVoidingMessageAsync().Returns(voidingMessages);
            ticketVoidingService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize).Returns(voidingMessages);
            ticketVoidingService.GetMessageInfoAsync(voidingMessages).Returns(messages);
            ticketVoidingService.GetTicketVoidingInfoAsync(voidingMessages).Returns(ticketInfos);
            ticketVoidingService.ImportTicketVoidingAsync(messages, ticketInfos).Returns(commandResult);

            // Act
            await service.PullMessageAsync(setting);

            // Assert
            await ticketVoidingService.Received().EditVoidingMessageAsync(voidingMessages);
        }

        #region Helpers

        private ChatBotJobService GetService()
        {
            logService = Substitute.For<ICRMLogService>();
            ticketVoidingService = Substitute.For<IChatBotTicketVoidingService>();
            return new ChatBotJobService(logService, ticketVoidingService);
        }

        #endregion Helpers
    }
}