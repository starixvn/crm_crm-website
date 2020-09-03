namespace CRM.Services.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Services;
    using CRM.Criteria.Setting;
    using CRM.Models.Setting;
    using NSubstitute;
    using Xunit;

    public class SettingServiceTests
    {
        private IBaseService baseService;

        [Fact]
        public async Task GetAsync_WhenCalled_ReturnExpectedListSettings()
        {
            var fixture = new Fixture();
            var expected = fixture.Create<IEnumerable<Setting>>();

            var service = GetService();

            baseService.GetDataAsync<Setting>(Arg.Any<GetSettingCriteria>()).Returns(expected);

            var actual = await service.GetAsync(fixture.Create<GetSettingParam>());

            Assert.Equal(expected, actual);
        }

        private SettingService GetService()
        {
            baseService = Substitute.For<IBaseService>();
            return new SettingService(baseService);
        }
    }
}