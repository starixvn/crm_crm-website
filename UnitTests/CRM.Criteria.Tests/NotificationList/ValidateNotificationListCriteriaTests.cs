namespace CRM.Criteria.Tests.NotificationList
{
    using CRM.Criteria.NotificationList;
    using Xunit;

    public class ValidateNotificationListCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new ValidateNotificationListCriteria();
            var expected = "Acc_CRM_ChatBot_NotificationListValidation";

            // Action
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(string notificationId)
        {
            // Arrange
            var criteria = new ValidateNotificationListCriteria
            {
                NotificationIds = notificationId,
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
            const string NotificationId = "NotificationId";
            var criteria = new ValidateNotificationListCriteria
            {
                NotificationIds = NotificationId,
            };

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}