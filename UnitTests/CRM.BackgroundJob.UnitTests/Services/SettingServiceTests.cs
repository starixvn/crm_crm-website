namespace CRM.BackgroundJob.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using Xunit;

    public class SettingServiceTests
    {
        private IBaseService baseService;

        [Fact]
        public async Task GetAllSettingAsync_SettingIsNotExist_ReturnEmpty()
        {
            // Arrange
            var settings = new List<SettingItem>();
            var service = GetSettingService();
            baseService.GetDataAsync<SettingItem>(Arg.Any<GetAllSettingCriteria>()).Returns(settings);

            // Act
            var actual = await service.GetAllSettingAsync();

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetAllSettingAsync_CheckSettingRelation_ReturnExpected()
        {
            // Arrange
            var settings = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, IsActive = true }
            };
            var relations = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, ParentId = 2 }
            };
            var service = GetSettingService();
            baseService.GetDataAsync<SettingItem>(Arg.Any<GetAllSettingCriteria>()).Returns(settings);
            baseService.GetDataAsync<SettingItem>(Arg.Any<GetAllSettingRelationCriteria>()).Returns(relations);

            // Act
            var actual = await service.GetAllSettingAsync();

            // Assert
            Assert.Equal(settings, actual);
            Assert.Single(settings.First().ParentIds);
            Assert.Equal(2, settings.First().ParentIds.First());
        }

        #region Helpers

        private SettingService GetSettingService()
        {
            baseService = Substitute.For<IBaseService>();
            return new SettingService(baseService);
        }

        #endregion Helpers
    }
}