namespace CRM.Criteria.Tests.NotificationList
{
    using CRM.Criteria.NotificationList;
    using Xunit;

    public class EditNotificationListCommandTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var command = new EditNotificationListCommand();
            var expected = "Acc_CRM_ChatBot_NotificationList_Upd";

            // Action
            var actual = command.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(int notificationId, int userId)
        {
            // Arrange
            var command = new EditNotificationListCommand
            {
                NotificationId = notificationId,
                UserId = userId
            };
            // Act
            var actual = command.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsValid_ParamsAreValid_ReturnTrue()
        {
            // Arrange
            var command = new EditNotificationListCommand
            {
                NotificationId = 1,
                UserId = 1
            };

            // Act
            var actual = command.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}