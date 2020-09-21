namespace CRM.BackgroundJob.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using Xunit;

    public class MessageServiceTests
    {
        private IBaseService baseService;

        [Fact]
        public async Task GetVoidingMessageAsync_WhenCalled_ReturnExpectedMessages()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<IEnumerable<VoidingMessage>>();
            var service = GetMessageService();
            baseService.GetDataAsync<VoidingMessage>(Arg.Any<GetVoidingMessageCriteria>()).Returns(expected);

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
            var expected = fixture.Create<IEnumerable<VoidingMessage>>();
            var batchSize = fixture.Create<int>();
            var voidingMessages = fixture.Create<IEnumerable<VoidingMessage>>();
            var xml = voidingMessages.ToXML("ROOT").ToString();
            var criteria = new ImportVoidingMessageCriteria();
            var service = GetMessageService();
            baseService.GetDataAsync<VoidingMessage>(Arg.Do<ImportVoidingMessageCriteria>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.ImportVoidingMessageAsync(voidingMessages, batchSize);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(xml, criteria.MessageIds);
            Assert.Equal(batchSize, criteria.BatchSize);
        }

        [Fact]
        public async Task GetFailMessageAsync_WhenCalled_ReturnExpectedMessages()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<IEnumerable<VoidingMessage>>();
            var service = GetMessageService();
            baseService.GetDataAsync<VoidingMessage>(Arg.Any<GetFailMessageCriteria>()).Returns(expected);

            // Act
            var actual = await service.GetFailMessageAsync();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetMessageInfoAsync_WhenCalled_ReturnExpectedMessages()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<IEnumerable<Message>>();
            var messageIds = fixture.Create<IEnumerable<int>>();
            var criteria = new GetMessageInfoCriteria();
            var service = GetMessageService();
            baseService.GetDataAsync<Message>(Arg.Do<GetMessageInfoCriteria>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.GetMessageInfoAsync(messageIds);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(string.Join(",", messageIds), criteria.MessageIds);
        }

        [Fact]
        public async Task EditStatusVoidingMessageAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<CommandResult>();
            var voidingMessages = fixture.Create<IEnumerable<VoidingMessage>>();
            var xml = voidingMessages.ToXML("ROOT").ToString();
            var criteria = new EditStatusVoidingMessageCommand();
            var service = GetMessageService();
            baseService.ExecuteNonQueryAsync(Arg.Do<EditStatusVoidingMessageCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.EditStatusVoidingMessageAsync(voidingMessages);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(xml, criteria.MessageIds);
        }

        #region Helpers

        private MessageService GetMessageService()
        {
            baseService = Substitute.For<IBaseService>();
            return new MessageService(baseService);
        }

        #endregion Helpers
    }
}