namespace CRM.Criteria.Tests.ChatTypeMapping
{
    using CRM.Criteria.ChatTypeMapping;
    using Xunit;

    public class GetChatTypeMappingCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetChatTypeMappingCriteria();
            var expected = "Acc_CRM_ChatBot_ChatTypeMapping_Sel";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", 0, 0)]
        [InlineData("", 1, 2)]
        [InlineData("a", 1, 0)]
        [InlineData("a", 0, 2)]
        public void IsValid_ParamsAreInvalid_ReturnFalse(string mappingStatus, int pageIndex, int pageSize)
        {
            // Arrange
            var criteria = new GetChatTypeMappingCriteria
            {
                MappingStatus = mappingStatus,
                PageIndex = pageIndex,
                PageSize = pageSize
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
            var criteria = new GetChatTypeMappingCriteria
            {
                MappingStatus = "AAA",
                PageSize = 100,
                PageIndex = 1
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}