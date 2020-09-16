namespace CRM.Criteria.Tests.Common
{
    using CRM.Criteria.Common;
    using Xunit;

    public class GetRecipientTypesCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetRecipientTypesCriteria();
            var expected = "Acc_CRM_ChatBot_RecipientType_Sel";

            // Action
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_ReturnTrue()
        {
            // Arrange
            var criteria = new GetRecipientTypesCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}