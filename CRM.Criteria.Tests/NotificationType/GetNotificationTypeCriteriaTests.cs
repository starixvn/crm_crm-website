namespace CRM.Criteria.Tests.NotificationType
{
    using CRM.Criteria.NotificationType;
    using Xunit;

    public class GetNotificationTypeCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetNotificationTypeCriteria();
            var expected = "Acc_CRM_ChatBot_NotificationType_Sel";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}