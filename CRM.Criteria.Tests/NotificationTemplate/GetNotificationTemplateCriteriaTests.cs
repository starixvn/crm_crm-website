namespace CRM.Criteria.Tests.NotificationTemplate
{
    using CRM.Criteria.NotificationTemplate;
    using Xunit;

    public class GetNotificationTemplateCriteriaTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new GetNotificationTemplateCriteria();
            var expected = "Acc_CRM_ChatBot_NotificationTemplate_Sel";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new GetNotificationTemplateCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}