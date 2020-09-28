namespace CRM.BackgroundJob.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using AutoFixture;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Criteria.ChatBot;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Models.ChatBot;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using Xunit;

    public class ChatBotTicketVoidingServiceTests
    {
        private IBaseService baseService;

        [Fact]
        public async Task GetVoidingMessageAsync_WhenCalled_ReturnExpectedMessages()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var service = GetService();
            baseService.GetDataAsync<ChatBotVoidingMessage>(Arg.Any<GetChatBotVoidingMessageCriteria>()).Returns(expected);

            // Act
            var actual = await service.GetVoidingMessageAsync();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ImportVoidingMessageAsync_WhenCalled_ReturnExpectedMessages()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var batchSize = fixture.Create<int>();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var xml = voidingMessages.ToXML("root").ToString();
            var criteria = new ImportChatBotVoidingMessageCriteria();
            var service = GetService();
            baseService.GetDataAsync<ChatBotVoidingMessage>(Arg.Do<ImportChatBotVoidingMessageCriteria>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.ImportVoidingMessageAsync(voidingMessages, batchSize);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(xml, criteria.Messages);
            Assert.Equal(batchSize, criteria.BatchSize);
        }

        [Fact]
        public async Task EditVoidingMessageAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<CommandResult>();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var xml = voidingMessages.ToXML("root").ToString();
            var criteria = new EditChatBotVoidingMessageCommand();

            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Do<EditChatBotVoidingMessageCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.EditVoidingMessageAsync(voidingMessages);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(xml, criteria.Messages);
        }

        [Fact]
        public async Task GetMessageInfoAsync_WhenCalled_ReturnExpectedMessages()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<IEnumerable<Message>>();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var xml = voidingMessages.ToXML("root").ToString();
            var criteria = new GetChatBotMessageInfoCriteria();
            var service = GetService();
            baseService.GetDataAsync<Message>(Arg.Do<GetChatBotMessageInfoCriteria>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.GetMessageInfoAsync(voidingMessages);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(xml, criteria.Messages);
        }

        [Fact]
        public async Task GetTicketVoidingInfoAsync_WhenCalled_ReturnExpectedTicketInfos()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<IEnumerable<ChatBotVoidingTicketInfo>>();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var xml = voidingMessages.ToXML("root").ToString();
            var criteria = new GetChatBotTicketVoidingInfoCriteria();
            var service = GetService();
            baseService.GetDataAsync<ChatBotVoidingTicketInfo>(Arg.Do<GetChatBotTicketVoidingInfoCriteria>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.GetTicketVoidingInfoAsync(voidingMessages);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(xml, criteria.Messages);
        }

        [Fact]
        public async Task ImportTicketVoidingAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            const string XmlTemplate = "<r MessageId='{0}' Username='{1}' CustId='{2}' " +
                "Recommend='{3}' Mrecommend='{4}' Srecommend='{5}' IsTest='{6}' " +
                "Content_en='{7}' Content_cs='{8}' Content_th='{9}' Content_vn='{10}' />";
            var fixture = new Fixture();
            var voidingMessages = fixture.Create<IEnumerable<ChatBotVoidingMessage>>();
            var messages = new List<Message>
            {
                fixture.Create<Message>()
            };
            var ticketInfos = new List<ChatBotVoidingTicketInfo>
            {
                fixture.Create<ChatBotVoidingTicketInfo>()
            };
            ticketInfos.First().MessageId = messages.First().MessageId;
            var ticketInfo = ticketInfos.First();
            var message = messages.First();
            var stringBuilder = new StringBuilder(Constants.BeginRootTag.ToLowerInvariant());
            stringBuilder.AppendFormat(XmlTemplate,
                message.MessageId,
                ticketInfo.Username,
                ticketInfo.CustId,
                ticketInfo.Recommend,
                ticketInfo.Mrecommend,
                ticketInfo.Srecommend,
                ticketInfo.IsTest,
                HttpUtility.HtmlEncode(message.Content_en),
                HttpUtility.HtmlEncode(message.Content_cs),
                HttpUtility.HtmlEncode(message.Content_th),
                HttpUtility.HtmlEncode(message.Content_vn));
            stringBuilder.Append(Constants.EndRootTag.ToLowerInvariant());
            var expected = fixture.Create<CommandResult>();
            var criteria = new ImportChatBotTicketVoidingInfoCommand();
            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Do<ImportChatBotTicketVoidingInfoCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.ImportTicketVoidingAsync(messages, ticketInfos, voidingMessages);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(stringBuilder.ToString(), criteria.XmlData);
            Assert.Equal(voidingMessages.ToXML("root").ToString(), criteria.Messages);
        }

        #region Helpers

        private ChatBotTicketVoidingService GetService()
        {
            baseService = Substitute.For<IBaseService>();
            return new ChatBotTicketVoidingService(baseService);
        }

        #endregion Helpers
    }
}