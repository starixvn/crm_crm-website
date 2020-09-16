namespace CRM.Letstalk.Services.Tests.Criteria
{
    using CRM.Letstalk.Services.Criteria;
    using Xunit;

    public class WriteLogMessageCommandTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new WriteLogMessageCommand();
            var expected = "Acc_CRM_ChatBot_NotificationMsgLog_Ins";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 1, "a", "b")]
        [InlineData(1, 0, "a", "b")]
        [InlineData(1, 1, "", "b")]
        [InlineData(1, 1, "a", "")]
        public void IsValid_WhenInvalidInputParams_ReturnFalse(int referrenceId, int botId, string letstalkId, string content)
        {
            // Arrange
            var criteria = new WriteLogMessageCommand { NotificationId = referrenceId, BotId = botId, ConversationId = letstalkId, Content = content };

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsValid_WhenValidInputParams_ReturnTrue()
        {
            // Arrange
            var criteria = new WriteLogMessageCommand { NotificationId = 1, BotId = 1, ConversationId = "a", Content = "b" };

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}