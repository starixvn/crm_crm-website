namespace CRM.Controllers.UnitTests
{
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Models.Auth;
    using CRM.Services;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Models;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class AuthControllerTests
    {
        private IUserService userService;
        private INapSession napSession;
        private ISecurityService securityService;
        private IAuthService authService;
        private AppSettings appSettings;
        private ExternalSiteHostUrl externalSiteHostUrl;
        private readonly Fixture fixture = new Fixture();

        #region SignIn

        [Fact]
        public async Task SignIn_InvalidUsername_ReturnExpectedViewResult()
        {
            // Arrange
            var model = new SignIn();
            var controller = GetController();

            //Act
            var actual = await controller.SignIn(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async Task SignIn_UserIsNull_ReturnExpectedRedirectResult()
        {
            // Arrange
            User user = null;
            var model = fixture.Create<SignIn>();
            var controller = GetController();
            userService.GetAsync(model.Username).Returns(user);

            //Act
            var actual = await controller.SignIn(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(actual);
            Assert.Equal(externalSiteHostUrl.DefaultPageUrl, redirectResult.Url);
        }

        [Fact]
        public async Task SignIn_InvalidPassword_ReturnExpectedRedirectResult()
        {
            // Arrange
            User user = null;
            var model = fixture.Create<SignIn>();
            var controller = GetController();
            userService.GetAsync(model.Username).Returns(user);
            authService.HashText(Arg.Any<string>()).Returns("invalid-password");

            //Act
            var actual = await controller.SignIn(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(actual);
            Assert.Equal(externalSiteHostUrl.DefaultPageUrl, redirectResult.Url);
        }

        #endregion SignIn

        #region Helpers

        private AuthController GetController()
        {
            userService = Substitute.For<IUserService>();
            napSession = Substitute.For<INapSession>();
            securityService = Substitute.For<ISecurityService>();
            authService = Substitute.For<IAuthService>();
            var settings = Substitute.For<IOptionsSnapshot<AppSettings>>();
            var externalUrl = Substitute.For<IOptionsSnapshot<ExternalSiteHostUrl>>();
            appSettings = new AppSettings
            {
                SingleSignOnExpiredTime = 30
            };
            externalSiteHostUrl = new ExternalSiteHostUrl
            {
                DefaultPageUrl = "home/index",
                SignInUrl = "auth/signin"
            };

            settings.Value.Returns(appSettings);
            externalUrl.Value.Returns(externalSiteHostUrl);
            return new AuthController(
                userService,
                napSession,
                securityService,
                authService,
                settings,
                externalUrl);
        }

        #endregion Helpers
    }
}