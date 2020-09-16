namespace CRM.Letstalk.Services.Tests.Criteria
{
    using CRM.Letstalk.Services.Criteria;
    using Xunit;

    public class GetNotificationToSendCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetNotificationToSendCriteria();
            var expected = "Acc_CRM_ChatBot_SendNotification";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_AllWaysReturnTrue()
        {
            // Arrange
            var criteria = new GetNotificationToSendCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}