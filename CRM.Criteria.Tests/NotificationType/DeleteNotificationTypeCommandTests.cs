namespace CRM.Criteria.Tests.NotificationType
{
    using CRM.Criteria.NotificationType;
    using Xunit;

    public class DeleteNotificationTypeCommandTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new DeleteNotificationTypeCommand();
            var expected = "Acc_CRM_ChatBot_NotificationType_Del";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(int notificationTypeId, int userId)
        {
            // Arrange
            var criteria = new DeleteNotificationTypeCommand
            {
                NotificationTypeId = notificationTypeId,
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
            var criteria = new DeleteNotificationTypeCommand
            {
                NotificationTypeId = 1,
                UserId = 8
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}
