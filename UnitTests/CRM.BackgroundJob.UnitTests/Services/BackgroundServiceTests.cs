namespace CRM.BackgroundJob.UnitTests.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Services;
    using CRM.Letstalk.Services.Services.Interfaces;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;
    using Xunit;

    public class BackgroundServiceTests
    {
        private IMaintenanceService maintenanceService;
        private IJobRunningChecker jobRunningChecker;
        private ISystemSettingService systemSettingService;
        private IAutoFeedbackService feedbackService;
        private IChatBotTicketVoidingService chatBotTicketVoidingService;
        private ICRMLogService logService;
        private ISendNotificationService sendNotificationService;
        private IMonitorService monitorService;

        #region CreateFeedbackForVoidingAbnormalBet

        [Fact]
        public async Task CreateFeedbackForVoidingAbnormalBet_Exception_ExpectedWriteLog()
        {
            // Arrange
            var fixture = new Fixture();
            var exception = fixture.Create<Exception>();
            var service = GetService();
            maintenanceService.When(x => x.GetUnderMaintenanceTimeAsync()).Throw(exception);

            // Act
            await service.CreateFeedbackForVoidingAbnormalBet();

            // Assert
            await jobRunningChecker.Received().ClearRunning(JobName.CreateFeedbackForVoidingAbnormalBet);
            await logService.Received().ErrorAsync(exception);
        }

        [Fact]
        public async Task CreateFeedbackForVoidingAbnormalBet_IsUnderMaintenance_ExpectedWriteLog()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = true };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);

            // Act
            await service.CreateFeedbackForVoidingAbnormalBet();

            // Assert
            logService.Received().LogInformation("UM time.");
        }

        [Fact]
        public async Task CreateFeedbackForVoidingAbnormalBet_ServiceIsShutdown_ExpectedWriteLog()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = false };
            var setting = new AbnormalBetSystemSetting { ServiceFlag = false };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);
            systemSettingService.GetAbnormalBetSettingAsync().Returns(setting);

            // Act
            await service.CreateFeedbackForVoidingAbnormalBet();

            // Assert
            logService.Received().LogInformation("Service is shutdown.");
        }

        [Fact]
        public async Task CreateFeedbackForVoidingAbnormalBet_ServiceIsRunning_ExpectedCreateFeedback()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = false };
            var setting = new AbnormalBetSystemSetting { ServiceFlag = true };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);
            systemSettingService.GetAbnormalBetSettingAsync().Returns(setting);

            // Act
            await service.CreateFeedbackForVoidingAbnormalBet();

            // Assert
            await feedbackService.Received().ExecuteJobAsync(setting);
        }

        #endregion CreateFeedbackForVoidingAbnormalBet

        #region PullMessageForChatBotAsync

        [Fact]
        public async Task PullMessageForChatBotAsync_Exception_ExpectedWriteLog()
        {
            // Arrange
            var fixture = new Fixture();
            var exception = fixture.Create<Exception>();
            var service = GetService();
            maintenanceService.When(x => x.GetUnderMaintenanceTimeAsync()).Throw(exception);

            // Act
            await service.PullMessageForChatBotAsync();

            // Assert
            await logService.Received().ErrorAsync(exception);
        }

        [Fact]
        public async Task PullMessageForChatBotAsync_IsUnderMaintenance_ExpectedWriteLog()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = true };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);

            // Act
            await service.PullMessageForChatBotAsync();

            // Assert
            logService.Received().LogInformation("UM time.");
        }

        [Fact]
        public async Task PullMessageForChatBotAsync_ServiceIsShutdown_ExpectedWriteLog()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = false };
            var setting = new AbnormalBetSystemSetting { ServiceFlag = false };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);
            systemSettingService.GetChatBotSystemSettingAsync().Returns(setting);

            // Act
            await service.PullMessageForChatBotAsync();

            // Assert
            logService.Received().LogInformation("Chat Bot Service is shutdown.");
        }

        [Fact]
        public async Task PullMessageForChatBotAsync_ServiceIsRunning_ExpectedPullMessageAndSendNotification()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = false };
            var setting = new AbnormalBetSystemSetting { ServiceFlag = true };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);
            systemSettingService.GetChatBotSystemSettingAsync().Returns(setting);

            // Act
            await service.PullMessageForChatBotAsync();

            // Assert
            await chatBotTicketVoidingService.Received().ExecuteJobAsync(setting);
            await sendNotificationService.Received().SendNotificationToLetstalk();
        }

        #endregion PullMessageForChatBotAsync

        #region ScanAndSenLetstalkNotification

        [Fact]
        public async Task ScanAndSenLetstalkNotification_Exception_ExpectedWriteLog()
        {
            // Arrange
            var fixture = new Fixture();
            var exception = fixture.Create<Exception>();
            var service = GetService();
            maintenanceService.When(x => x.GetUnderMaintenanceTimeAsync()).Throw(exception);

            // Act
            await service.ScanAndSenLetstalkNotification();

            // Assert
            await logService.Received().ErrorAsync(exception);
        }

        [Fact]
        public async Task ScanAndSenLetstalkNotification__IsUnderMaintenance_ExpectedWriteLog()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = true };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);

            // Act
            await service.ScanAndSenLetstalkNotification();

            // Assert
            logService.Received().LogInformation("UM time.");
        }

        [Fact]
        public async Task ScanAndSenLetstalkNotification_WhenCalled_ExpectedSendNotification()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = false };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);

            // Act
            await service.ScanAndSenLetstalkNotification();

            // Assert
            await sendNotificationService.Received().SendNotificationToLetstalk();
        }

        #endregion ScanAndSenLetstalkNotification

        [Fact]
        public async Task MonitorLetstalkService_ExpectedCallMonitorLetstalkService()
        {
            // Arrange
            var service = GetService();

            // Act
            await service.MonitorLetstalkService();

            // Assert
            await monitorService.Received().MonitorLetstalkService();
        }

        #region Helpers

        private BackgroundService GetService()
        {
            maintenanceService = Substitute.For<IMaintenanceService>();
            jobRunningChecker = Substitute.For<IJobRunningChecker>();
            systemSettingService = Substitute.For<ISystemSettingService>();
            feedbackService = Substitute.For<IAutoFeedbackService>();
            chatBotTicketVoidingService = Substitute.For<IChatBotTicketVoidingService>();
            logService = Substitute.For<ICRMLogService>();
            sendNotificationService = Substitute.For<ISendNotificationService>();
            monitorService = Substitute.For<IMonitorService>();

            return new BackgroundService(
                maintenanceService,
                jobRunningChecker,
                systemSettingService,
                feedbackService,
                chatBotTicketVoidingService,
                logService,
                sendNotificationService,
                monitorService);
        }

        #endregion Helpers
    }
}