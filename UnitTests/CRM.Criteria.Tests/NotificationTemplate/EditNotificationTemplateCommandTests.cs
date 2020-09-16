namespace CRM.Criteria.Tests.NotificationTemplate
{
    using CRM.Criteria.NotificationTemplate;
    using Xunit;

    public class EditNotificationTemplateCommandTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new EditNotificationTemplateCommand();
            var expected = "Acc_CRM_ChatBot_NotificationTemplate_Upd";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 0)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 1, 0)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(int userId, int notificationTemplateId, int notificationTypeId)
        {
            // Arrange
            var criteria = new EditNotificationTemplateCommand
            {
                UserId = userId,
                NotificationTemplateId = notificationTemplateId,
                NotificationTypeId = notificationTypeId
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsValid_ParamsAreValid_ReturnTrue()
        {
            // Arrange
            var criteria = new EditNotificationTemplateCommand
            {
                UserId = 1,
                NotificationTemplateId = 1,
                NotificationTypeId = 1
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}