namespace CRM.BackgroundJob.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Services;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class AutoFeedbackServiceTests
    {
        private AppJobSettings appSettings;
        private IJobRunningChecker jobRunningChecker;
        private ISystemSettingService systemSettingService;
        private IFeedbackService feedbackService;
        private ICRMLogService logService;
        private readonly IFixture fixture = new Fixture();

        [Fact]
        public async Task ScanAndSenLetstalkNotification_Exception_ExpectedWriteLogAndClearJob()
        {
            // Arrange
            var exception = fixture.Create<Exception>();
            var service = GetService();
            systemSettingService.When(x => x.GetAbnormalBetSettingAsync()).Throw(exception);

            // Act
            await service.ExecuteJobAsync();

            // Assert
            await logService.Received().ErrorAsync(exception);
            await jobRunningChecker.Received().ClearRunning(JobName.CreateFeedbackForVoidingAbnormalBet);
        }

        [Fact]
        public async Task ExecuteJobAsync_ServiceIsShutdown_ExpectedWriteLog()
        {
            // Arrange
            var setting = new AbnormalBetSystemSetting { ServiceFlag = false };
            var service = GetService();
            systemSettingService.GetAbnormalBetSettingAsync().Returns(setting);

            // Act
            await service.ExecuteJobAsync();

            // Assert
            logService.Received().LogInformation("Service is shutdown.");
        }

        [Fact]
        public async Task ExecuteJobAsync_JobIsRunningAndLastRuntimeIsLowerThanNow_DoNothing()
        {
            // Arrange
            var setting = new AbnormalBetSystemSetting
            {
                ServiceFlag = true,
                LastRuntime = DateTime.Now,
                JobStatus = JobStatus.Running
            };
            var service = GetService();
            systemSettingService.GetAbnormalBetSettingAsync().Returns(setting);
            jobRunningChecker.IsRunning(JobName.CreateFeedbackForVoidingAbnormalBet).Returns(true);

            // Assert
            await service.ExecuteJobAsync();

            // Act
            logService.DidNotReceive().LogInformation(Arg.Any<string>());
            await systemSettingService.DidNotReceive().EditSystemSettingAsync(Arg.Any<IEnumerable<SystemSettingItem>>());
        }

        [Fact]
        public async Task ExecuteJobAsync_JobIsRunningAndLastRuntimeIsGreaterThanNow_DoNothing()
        {
            // Arrange
            var service = GetService();
            var setting = new AbnormalBetSystemSetting
            {
                ServiceFlag = true,
                LastRuntime = DateTime.Now.AddMinutes(-appSettings.RetryRuntime),
                JobStatus = JobStatus.Running
            };
            systemSettingService.GetAbnormalBetSettingAsync().Returns(setting);
            jobRunningChecker.IsRunning(JobName.CreateFeedbackForVoidingAbnormalBet).Returns(false);

            // Assert
            await service.ExecuteJobAsync();

            // Act
            logService.Received().LogInformation(Arg.Any<string>());
            await systemSettingService.Received().EditSystemSettingAsync(Arg.Any<IEnumerable<SystemSettingItem>>());
        }

        [Fact]
        public async Task ExecuteJobAsync_JobIsCompleted_ExpectedCallCreateFeedback()
        {
            // Arrange
            var service = GetService();
            var setting = new AbnormalBetSystemSetting
            {
                ServiceFlag = true,
                JobStatus = JobStatus.Completed
            };
            systemSettingService.GetAbnormalBetSettingAsync().Returns(setting);

            // Assert
            await service.ExecuteJobAsync();

            // Assert
            await jobRunningChecker.Received().MarkRunning(JobName.CreateFeedbackForVoidingAbnormalBet);
            await jobRunningChecker.Received().ClearRunning(JobName.CreateFeedbackForVoidingAbnormalBet);
            await feedbackService.Received().CreateFeedbackForVoidingAbnormalBetAsync(setting);
        }

        #region Helpers

        private AutoFeedbackService GetService()
        {
            appSettings = new AppJobSettings
            {
                RetryRuntime = 5
            };
            var appJobSettings = Substitute.For<IOptionsSnapshot<AppJobSettings>>();
            appJobSettings.Value.Returns(appSettings);
            jobRunningChecker = Substitute.For<IJobRunningChecker>();
            systemSettingService = Substitute.For<ISystemSettingService>();
            feedbackService = Substitute.For<IFeedbackService>();
            logService = Substitute.For<ICRMLogService>();
            return new AutoFeedbackService(
                appJobSettings,
                jobRunningChecker,
                systemSettingService,
                feedbackService,
                logService);
        }

        #endregion Helpers
    }
}