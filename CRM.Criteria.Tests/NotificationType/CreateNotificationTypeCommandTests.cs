namespace CRM.Criteria.Tests.NotificationType
{
    using Xunit;
    using CRM.Criteria.NotificationType;

    public class CreateNotificationTypeCommandTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new CreateNotificationTypeCommand();
            var expected = "Acc_CRM_ChatBot_NotificationType_Ins";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "", "", 0, 0)]
        [InlineData("1,2", "", "1", 1, 1)]
        [InlineData("", "", "", 1, 1)]
        [InlineData("Name", "1, 2", "2", 0, 0)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(string notificationTypeName, string chatTypeIds, string recipientTypeIds, int systemId, int userId)
        {
            // Arrange
            var criteria = new CreateNotificationTypeCommand
            {
                NotificationTypeName = notificationTypeName,
                ChatTypeIds = chatTypeIds,
                RecipientTypeIds = recipientTypeIds,
                SystemId = systemId,
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
            var criteria = new CreateNotificationTypeCommand
            {
                NotificationTypeName = "Notificaton Type Name",
                ChatTypeIds = "1,2",
                RecipientTypeIds = "2",
                SystemId = 1,
                UserId = 8
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}
