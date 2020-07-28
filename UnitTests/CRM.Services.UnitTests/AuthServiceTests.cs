namespace CRM.Services.UnitTests
{
    using System.Threading.Tasks;
    using CRM.Core.Enumerables;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using NSubstitute;
    using ONEbook.UM.Services;
    using Xunit;

    public class AuthServiceTests
    {
        private INapSession napSession;
        private INapSettings napSettings;
        private ISiteService siteService;
        private ISecurityService securityService;
        private IMaintenanceService maintenanceService;

        [Fact]
        public async Task GetRecheckInStatusAsync_IsUnderMaintenance_ReturnExpectedStatus()
        {
            // Arrange
            var service = GetAuthService();
            maintenanceService.IsUnderMaintenanceAsync().Returns(true);

            // Act
            var actual = await service.GetRecheckInStatusAsync();

            // Assert
            Assert.Equal(RecheckInStatus.UnderMaintenance, actual);
        }

        [Fact]
        public async Task GetRecheckInStatusAsync_DisabledReCheckIn_ReturnExpectedStatus()
        {
            // Arrange
            var service = GetAuthService();
            maintenanceService.IsUnderMaintenanceAsync().Returns(false);
            napSettings.EnableReCheckIn.Returns(false);

            // Act
            var actual = await service.GetRecheckInStatusAsync();

            // Assert
            Assert.Equal(RecheckInStatus.Success, actual);
        }

        [Fact]
        public async Task GetRecheckInStatusAsync_OnUserIdIsInvalid_ReturnExpectedStatus()
        {
            // Arrange
            var service = GetAuthService();
            maintenanceService.IsUnderMaintenanceAsync().Returns(false);
            napSettings.EnableReCheckIn.Returns(true);
            napSession.OnUserId.Returns(0);

            // Act
            var actual = await service.GetRecheckInStatusAsync();

            // Assert
            Assert.Equal(RecheckInStatus.Success, actual);
        }

        [Fact]
        public async Task GetRecheckInStatusAsync_SiteIsDeactivated_ReturnExpectedStatus()
        {
            // Arrange
            var service = GetAuthService();
            maintenanceService.IsUnderMaintenanceAsync().Returns(false);
            napSettings.EnableReCheckIn.Returns(true);
            napSession.OnUserId.Returns(1);
            siteService.IsActivatedAsync(Arg.Any<int>()).Returns(false);

            // Act
            var actual = await service.GetRecheckInStatusAsync();

            // Assert
            Assert.Equal(RecheckInStatus.InvalidSite, actual);
        }

        [Fact]
        public async Task GetRecheckInStatusAsync_IsKickOut_ReturnExpectedStatus()
        {
            // Arrange
            var service = GetAuthService();
            maintenanceService.IsUnderMaintenanceAsync().Returns(false);
            napSettings.EnableReCheckIn.Returns(true);
            napSession.OnUserId.Returns(1);
            siteService.IsActivatedAsync(Arg.Any<int>()).Returns(true);
            securityService.ReCheckInAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(1);

            // Act
            var actual = await service.GetRecheckInStatusAsync();

            // Assert
            Assert.Equal(RecheckInStatus.KickOut, actual);
        }

        [Fact]
        public async Task GetRecheckInStatusAsync_IsSuccess_ReturnExpectedStatus()
        {
            // Arrange
            var service = GetAuthService();
            maintenanceService.IsUnderMaintenanceAsync().Returns(false);
            napSettings.EnableReCheckIn.Returns(true);
            napSession.OnUserId.Returns(1);
            siteService.IsActivatedAsync(Arg.Any<int>()).Returns(true);
            securityService.ReCheckInAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(0);

            // Act
            var actual = await service.GetRecheckInStatusAsync();

            // Assert
            Assert.Equal(RecheckInStatus.Success, actual);
        }

        #region Helpers

        private AuthService GetAuthService()
        {
            napSession = Substitute.For<INapSession>();
            napSettings = Substitute.For<INapSettings>();
            siteService = Substitute.For<ISiteService>();
            securityService = Substitute.For<ISecurityService>();
            maintenanceService = Substitute.For<IMaintenanceService>();
            var service = new AuthService(napSession, napSettings, siteService, securityService, maintenanceService);

            return service;
        }

        #endregion Helpers
    }
}