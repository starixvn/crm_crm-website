namespace CRM.Criteria.Tests.NotificationType
{
    using CRM.Criteria.NotificationType;
    using Xunit;

    public class EditNotificationTypeCommandTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new EditNotificationTypeCommand();
            var expected = "Acc_CRM_ChatBot_NotificationType_Upd";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData( "", "", "", 0, 0, 0)]
        [InlineData("Name", "", "1", 1, 1, 0)]
        [InlineData("", "", "", 1, 1, 1)]
        [InlineData("Name", "1,2", "2", 0, 0, 0)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(string notificationTypeName, string chatTypeIds, string recipientTypeIds, int systemId, int userId, int notificationTypeId)
        {
            // Arrange
            var criteria = new EditNotificationTypeCommand
            {
                NotificationTypeId = notificationTypeId,
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
            var criteria = new EditNotificationTypeCommand
            {
                NotificationTypeId = 3,
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
