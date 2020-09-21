namespace CRM.BackgroundJob.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
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

        [Fact]
        public async Task ExecuteJobAsync_JobIsRunningAndLastRuntimeIsLowerThanNow_DoNothing()
        {
            // Arrange
            var setting = new AbnormalBetSystemSetting
            {
                LastRuntime = DateTime.Now,
                JobStatus = JobStatus.Running
            };
            var service = GetService();
            jobRunningChecker.IsRunning(JobName.CreateFeedbackForVoidingAbnormalBet).Returns(true);

            // Assert
            await service.ExecuteJobAsync(setting);

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
                LastRuntime = DateTime.Now.AddMinutes(-appSettings.RetryRuntime),
                JobStatus = JobStatus.Running
            };
            jobRunningChecker.IsRunning(JobName.CreateFeedbackForVoidingAbnormalBet).Returns(false);

            // Assert
            await service.ExecuteJobAsync(setting);

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
                JobStatus = JobStatus.Completed
            };

            // Assert
            await service.ExecuteJobAsync(setting);

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