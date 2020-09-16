namespace CRM.Criteria.Tests.ChatTypeMapping
{
    using CRM.Criteria.ChatTypeMapping;
    using Xunit;

    public class UpdateActiveStatusCommandTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new UpdateIsActiveStatusCommand();
            var expected = "Acc_CRM_ChatBot_ChatTypeMappingStatus_Upd";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(int conversationId, int userId)
        {
            // Arrange
            var criteria = new UpdateIsActiveStatusCommand
            {
                ConversationGroupId = conversationId,
                UserId = userId,
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
            var criteria = new UpdateIsActiveStatusCommand
            {
                ConversationGroupId = 1,
                UserId = 1,
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}