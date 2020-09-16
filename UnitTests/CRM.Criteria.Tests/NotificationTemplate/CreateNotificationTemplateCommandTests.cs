namespace CRM.Criteria.Tests.NotificationTemplate
{
    using CRM.Criteria.NotificationTemplate;
    using Xunit;

    public class CreateNotificationTemplateCommandTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new CreateNotificationTemplateCommand();
            var expected = "Acc_CRM_ChatBot_NotificationTemplate_Ins";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(int userId, int notificationTypeId)
        {
            // Arrange
            var criteria = new CreateNotificationTemplateCommand
            {
                UserId = userId,
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
            var criteria = new CreateNotificationTemplateCommand
            {
                UserId = 1,
                NotificationTypeId = 1
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}