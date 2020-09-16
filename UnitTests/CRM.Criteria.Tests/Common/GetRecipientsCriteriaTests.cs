namespace CRM.Criteria.Tests.Common
{
    using CRM.Criteria.Common;
    using Xunit;

    public class GetRecipientsCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetRecipientsCriteria();
            var expected = "Acc_CRM_ChatBot_Recipient_Sel";

            // Action
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_ReturnTrue()
        {
            // Arrange
            var criteria = new GetRecipientsCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}