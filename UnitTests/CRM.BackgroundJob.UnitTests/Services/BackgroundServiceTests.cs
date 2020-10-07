namespace CRM.BackgroundJob.UnitTests.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Services;
    using CRM.Letstalk.Services.Services.Interfaces;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;
    using Xunit;

    public class BackgroundServiceTests
    {
        private IMaintenanceService maintenanceService;
        private IAutoFeedbackService feedbackService;
        private IChatBotJobService chatBotJobService;
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
        public async Task CreateFeedbackForVoidingAbnormalBet_WhenCalled_ExpectedCreateFeedback()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = false };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);

            // Act
            await service.CreateFeedbackForVoidingAbnormalBet();

            // Assert
            await feedbackService.Received().ExecuteJobAsync();
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
        public async Task PullMessageForChatBotAsync_WhenCalled_ExpectedPullMessageAndSendNotification()
        {
            // Arrange
            var underMaintenanceTime = new UnderMaintenanceTime { IsUnderMaintenanceTime = false };
            var service = GetService();
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(underMaintenanceTime);

            // Act
            await service.PullMessageForChatBotAsync();

            // Assert
            await chatBotJobService.Received().PullMessageAndSendNotificationAsync();
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
        public async Task MonitorLetstalkService_WhenCalled_ExpectedCallMonitorLetstalkService()
        {
            // Arrange
            var service = GetService();

            // Act
            await service.MonitorLetstalkService();

            // Assert
            await monitorService.Received().MonitorLetstalkService();
        }

        [Fact]
        public async Task MonitorLetstalkService_Exception_ExpectedWriteLog()
        {
            // Arrange
            var fixture = new Fixture();
            var exception = fixture.Create<Exception>();
            var service = GetService();
            monitorService.When(x => x.MonitorLetstalkService()).Throw(exception);

            // Act
            await service.MonitorLetstalkService();

            // Assert
            await logService.Received().ErrorAsync(exception);
        }

        #region Helpers

        private BackgroundService GetService()
        {
            maintenanceService = Substitute.For<IMaintenanceService>();
            feedbackService = Substitute.For<IAutoFeedbackService>();
            chatBotJobService = Substitute.For<IChatBotJobService>();
            logService = Substitute.For<ICRMLogService>();
            sendNotificationService = Substitute.For<ISendNotificationService>();
            monitorService = Substitute.For<IMonitorService>();

            return new BackgroundService(
                maintenanceService,
                feedbackService,
                chatBotJobService,
                logService,
                sendNotificationService,
                monitorService);
        }

        #endregion Helpers
    }
}