namespace CRM.BackgroundJob.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Criteria.ChatBot;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using Xunit;

    public class SystemSettingServiceTests
    {
        private IBaseService baseService;

        [Fact]
        public async Task GetAbnormalBetSettingAsync_WhenCalled_ReturnExpectedSetting()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<AbnormalBetSystemSetting>();
            var service = GetSystemSettingService();
            baseService.GetDataEntityAsync<AbnormalBetSystemSetting>(Arg.Any<GetAbnormalBetSystemSettingCriteria>()).Returns(expected);

            // Act
            var actual = await service.GetAbnormalBetSettingAsync();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task EditSystemSettingAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<CommandResult>();
            var settings = fixture.Create<IEnumerable<SystemSettingItem>>();
            var xml = settings.ToXML("ROOT").ToString();
            var criteria = new EditSystemSettingCommand();
            var service = GetSystemSettingService();
            baseService.ExecuteNonQueryAsync(Arg.Do<EditSystemSettingCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.EditSystemSettingAsync(settings);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(xml, criteria.Settings);
        }

        [Fact]
        public async Task EditJobStatusAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var status = fixture.Create<string>();
            var expected = fixture.Create<CommandResult>();
            var settings = new List<SystemSettingItem>
            {
                new SystemSettingItem { SettingName = SystemSettingName.JobStatus, Value = status }
            };
            var xml = settings.ToXML("ROOT").ToString();
            var criteria = new EditSystemSettingCommand();
            var service = GetSystemSettingService();
            baseService.ExecuteNonQueryAsync(Arg.Do<EditSystemSettingCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.EditJobStatusAsync(status);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(xml, criteria.Settings);
        }

        [Fact]
        public async Task GetChatBotSystemSettingAsync_WhenCalled_ReturnExpectedSetting()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<AbnormalBetSystemSetting>();
            var service = GetSystemSettingService();
            baseService.GetDataEntityAsync<AbnormalBetSystemSetting>(Arg.Any<GetChatBotSystemSettingCriteria>()).Returns(expected);

            // Act
            var actual = await service.GetChatBotSystemSettingAsync();

            // Assert
            Assert.Equal(expected, actual);
        }

        #region Helpers

        private SystemSettingService GetSystemSettingService()
        {
            baseService = Substitute.For<IBaseService>();
            return new SystemSettingService(baseService);
        }

        #endregion Helpers
    }
}