namespace CRM.Letstalk.Services.Tests.Criteria
{
    using CRM.Letstalk.Services.Criteria;
    using Xunit;

    public class UpdateNotificationStatusCommandTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new UpdateNotificationStatusCommand();
            var expected = "Acc_CRM_ChatBot_SendNotification_Status_Upd";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_WhenParamIsNullOrWhiteSpace_ReturnFalse()
        {
            // Arrange
            var criteria = new UpdateNotificationStatusCommand { XmlData = string.Empty };

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsValid_WhenParamIsNotNullOrWhiteSpace_ReturnTrue()
        {
            // Arrange
            var criteria = new UpdateNotificationStatusCommand { XmlData = "XML" };

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}