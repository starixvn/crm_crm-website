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

    public class ChatTypeMappingServiceTests
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
            var actual = await service.EditActiveStatusAsync(1, true, 10);

            // Assert
            Assert.Equal(result, actual);
        }

        [Fact]
        public async Task GetHeadlineByRecipientAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var groupHeadlines = new List<GroupHeadlineDirectAccountModel>
            {
                new GroupHeadlineDirectAccountModel  { Id = 0, Name = "Not Assigned Group" },
                new GroupHeadlineDirectAccountModel  { Id = 1, Name = "HL01" },
                new GroupHeadlineDirectAccountModel  { Id = 2, Name = "HL02" },
            };
            var notAssignedGroupDirectAccounts = new List<string> { "DR1", "DR2" };
            var service = GetService();
            baseService.FetchMultipleAsync<GroupHeadlineDirectAccountModel, string>(Arg.Any<GetGroupHeadlineByRecipientCriteria>())
                .Returns(new ValueTuple<IEnumerable<GroupHeadlineDirectAccountModel>, IEnumerable<string>>(groupHeadlines, notAssignedGroupDirectAccounts));
            // Act

            var actual = await service.GetHeadlineByRecipientAsync("1,2,3,4");
            groupHeadlines.FirstOrDefault(g => g.Id == 0).SubData = notAssignedGroupDirectAccounts;
            // Assert
            Assert.Equal(groupHeadlines, actual);
        }

        [Fact]
        public async Task InsertPersonalMappingAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<CommandResult>();

            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Any<InsertPersonalMappingCommand>()).Returns(result);
            // Act
            var actual = await service.CreatePersonalMappingAsync(fixture.Create<InsertPersonalMappingCommand>());

            // Assert
            Assert.Equal(result, actual);
        }

        [Fact]
        public async Task UpdateChatTypeMappingAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var result = fixture.Create<CommandResult>();

            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Any<UpdateChatTypeMappingCommand>()).Returns(result);
            // Act
            var actual = await service.EditChatTypeMappingAsync(fixture.Create<UpdateChatTypeMappingCommand>());

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