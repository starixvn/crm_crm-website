namespace CRM.Criteria.Tests.NotificationList
{
    using CRM.Criteria.NotificationList;
    using Xunit;

    public class GetNotificationListCriteriaTests
    {
        private const string Error = "Error";
        private const string TypeId = "TypeId";

        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetNotificationListCriteria();
            var expected = "Acc_CRM_ChatBot_NotificationList_Sel";

            // Action
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", TypeId)]
        [InlineData(null, TypeId)]
        [InlineData(Error, "")]
        [InlineData(Error, null)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(string notificationTypeIds, string status)
        {
            // Arrange
            var criteria = new GetNotificationListCriteria
            {
                NotificationTypeIds = notificationTypeIds,
                Status = status
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
            var criteria = new GetNotificationListCriteria
            {
                NotificationTypeIds = TypeId,
                Status = Error
            };

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}