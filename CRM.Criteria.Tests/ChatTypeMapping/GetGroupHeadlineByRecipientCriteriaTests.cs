namespace CRM.Criteria.Tests.ChatTypeMapping
{
    using CRM.Criteria.ChatTypeMapping;
    using Xunit;

    public class GetGroupHeadlineByRecipientCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetGroupHeadlineByRecipientCriteria();
            var expected = "Acc_CRM_GetHeadlineByRecipient";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_ParamsAreInvalid_ReturnFalse()
        {
            // Arrange
            var criteria = new GetGroupHeadlineByRecipientCriteria
            {
                RecipientIds = string.Empty
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsValid_ParamsArevalid_ReturnTrue()
        {
            // Arrange
            var criteria = new GetGroupHeadlineByRecipientCriteria
            {
                RecipientIds = "1,2,3,4"
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}