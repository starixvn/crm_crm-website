namespace CRM.Criteria.Tests.NotificationTemplate
{
    using CRM.Criteria.NotificationTemplate;
    using Xunit;

    public class DeleteNotificationTemplateCommandTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new DeleteNotificationTemplateCommand();
            var expected = "Acc_CRM_ChatBot_NotificationTemplate_Del";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(int userId, int notificationTemplateId)
        {
            // Arrange
            var criteria = new DeleteNotificationTemplateCommand
            {
                UserId = userId,
                NotificationTemplateId = notificationTemplateId
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
            var criteria = new DeleteNotificationTemplateCommand
            {
                UserId = 1,
                NotificationTemplateId = 1
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}