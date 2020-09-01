namespace CRM.Services.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.ChatTypeMapping;
    using CRM.Models.ChatTypeMapping;
    using NSubstitute;
    using Xunit;

    public class ChatTypeServiceTests
    {
        private IBaseService baseService;

        [Fact]
        public async Task GetChatTypeMappingAsync_WhenCalled_ReturnExpectedListMapping()
        {
            // Arrange
            var fixture = new Fixture();
            var totalRows = fixture.Create<int>();
            var result = fixture.Create<ValueTuple<IEnumerable<ChatTypeMappingModel>, IEnumerable<ConversationHeadlineModel>, IEnumerable<ConversationRecipient>>>();
            foreach (var item in result.Item1)
            {
                item.Headlines = result.Item2.Where(headline => headline.ConversationGroupId == item.ConversationGroupId);
                item.Recipients = result.Item3.Where(recipient => recipient.ConversationGroupId == item.ConversationGroupId);
            }
            var criteria = new GetChatTypeMappingCriteria();
            var service = GetService();
            baseService.FetchMultipleAsync<ChatTypeMappingModel, ConversationHeadlineModel, ConversationRecipient>(Arg.Do<GetChatTypeMappingCriteria>(x => criteria = x)).Returns(result);
            baseService.When(x => x.FetchMultipleAsync<ChatTypeMappingModel, ConversationHeadlineModel, ConversationRecipient>(Arg.Do<GetChatTypeMappingCriteria>(y => criteria = y)))
                 .Do(x => criteria.TotalRows = totalRows);

            // Act
            var actual = await service.GetChatTypeMappingAsync(criteria);

            // Assert
            Assert.Equal(result.Item1, actual.Item1);
            Assert.Equal(actual.Item2, criteria.TotalRows);
        }

        [Fact]
        public async Task UpdateActiveStatusAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<CommandResult>();

            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Any<UpdateIsActiveStatusCommand>()).Returns(result);
            // Act
            var actual = await service.UpdateActiveStatusAsync(1, true, 10);

            // Assert
            Assert.Equal(result, actual);
        }

        #region Helpers

        private ChatTypeMappingService GetService()
        {
            baseService = Substitute.For<IBaseService>();
            var service = new ChatTypeMappingService(baseService);

            return service;
        }

        #endregion Helpers
    }
}