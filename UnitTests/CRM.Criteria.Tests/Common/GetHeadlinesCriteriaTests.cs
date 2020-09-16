namespace CRM.Criteria.Tests.Common
{
    using CRM.Criteria.Common;
    using Xunit;

    public class GetHeadlinesCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetHeadlinesCriteria();
            var expected = "Acc_CRM_HeadlineSel";

            // Action
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_ReturnTrue()
        {
            // Arrange
            var criteria = new GetHeadlinesCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}