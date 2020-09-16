namespace CRM.Criteria.Tests.ChatTypeMapping
{
    using CRM.Criteria.ChatTypeMapping;
    using Xunit;

    public class InsertPersonalMappingCommandTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new InsertPersonalMappingCommand();
            var expected = "Acc_CRM_ChatBot_ChatTypeMapping_Ins";

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
            var criteria = new InsertPersonalMappingCommand
            {
                BotId = botId,
                ConversationGroupName = conversationName,
                ChatTypeId = chatTypeId,
                RecipientTypeId = recipientTypeId,
                HeadlineIds = headlineIds,
                RecipientId = recipientId,
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
            var criteria = new InsertPersonalMappingCommand
            {
                BotId = 1,
                ConversationGroupName = "a",
                ChatTypeId = 1,
                RecipientTypeId = 1,
                HeadlineIds = "abcx",
                RecipientId = 1,
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