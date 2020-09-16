namespace CRM.Criteria.Tests.ChatTypeMapping
{
    using CRM.Criteria.ChatTypeMapping;
    using Xunit;

    public class UpdateChatTypeMappingCommandTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new UpdateChatTypeMappingCommand();
            var expected = "Acc_CRM_ChatBot_ChatTypeMapping_Upd";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, "", "", "", 0, 0)]
        [InlineData(0, 1, 0, 0, "1", "", "", 0, 0)]
        [InlineData(0, 1, 2, 3, "1", "", "", 4, 5)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(int botId, int chatTypeId, int recipientTypeId, int recipientId,
            string conversationName, string headlineIds, string letstalkId, int langId, int userId)
        {
            // Arrange
            var criteria = new UpdateChatTypeMappingCommand
            {
                BotId = botId,
                ConversationGroupName = conversationName,
                RecipientTypeId = recipientTypeId,
                HeadlineIds = headlineIds,
                RecipientIds = recipientId.ToString(),
                LetstalkId = letstalkId,
                LanguageId = langId,
                UserId = userId
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsValid_ParamsArevalid_ReturnTrue()
        {
            // Arrange
            var criteria = new UpdateChatTypeMappingCommand
            {
                ConversationGroupId = 1,
                BotId = 1,
                ConversationGroupName = "a",
                RecipientTypeId = 1,
                HeadlineIds = "abcx",
                RecipientIds = "abc",
                LetstalkId = "ABC",
                LanguageId = 2,
                UserId = 10
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}