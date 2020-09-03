namespace CRM.Controllers.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Models.Auth;
    using CRM.Services;
    using Fanex.Data;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Models;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
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
        private const int SingleSignOnExpiredTime = 30;

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

        [Fact]
        public async Task SignIn_UserIsExist_ReturnExpectedRedirectResult()
        {
            // Arrange
            var model = fixture.Create<SignIn>();
            model.OnUserId = Cryptography.EncryptPassword("1");
            var user = new User
            {
                Id = 1,
                Username = "sa"
            };
            var controller = GetController();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = MockServiceProvider()
                }
            };
            userService.GetAsync(model.Username).Returns(user);
            authService.HashText(Arg.Any<string>()).Returns(model.Password);

            //Act
            var actual = await controller.SignIn(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(actual);
            Assert.Equal(externalSiteHostUrl.DefaultPageUrl, redirectResult.Url);
            napSession.UserId = user.Id;
            napSession.UserName = user.Username;
            napSession.OnUserId = 1;
        }

        #endregion SignIn

        #region SingleSignOn

        [Fact]
        public async Task SingleSignOn_InvalidTime_ReturnExpectedRedirectResult()
        {
            // Arrange
            var time = DateTime.Now;
            var expirationTime = time.AddSeconds(SingleSignOnExpiredTime + 1);
            var model = fixture.Create<SingleSignOn>();
            model.Time = Cryptography.EncryptPassword(time.ToLongTimeString());
            var controller = GetController();
            securityService.GetTime().Returns(Cryptography.EncryptPassword(expirationTime.ToLongTimeString()));

            //Act
            var actual = await controller.SingleSignOn(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(actual);
            Assert.Equal(externalSiteHostUrl.DefaultPageUrl, redirectResult.Url);
        }

        [Fact]
        public async Task SingleSignOn_UserIsNull_ReturnExpectedRedirectResult()
        {
            // Arrange
            var time = DateTime.Now;
            var expirationTime = time.AddSeconds(SingleSignOnExpiredTime - 1);
            User user = null;
            var model = new SingleSignOn
            {
                UserId = Cryptography.EncryptPassword("1"),
                Time = Cryptography.EncryptPassword(time.ToLongTimeString())
            };
            model.Time = Cryptography.EncryptPassword(time.ToLongTimeString());
            var controller = GetController();
            securityService.GetTime().Returns(Cryptography.EncryptPassword(expirationTime.ToLongTimeString()));
            userService.GetAsync(1).Returns(user);

            //Act
            var actual = await controller.SingleSignOn(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(actual);
            Assert.Equal(externalSiteHostUrl.DefaultPageUrl, redirectResult.Url);
        }

        [Fact]
        public async Task SingleSignOn_UserIsExist_ReturnExpectedRedirectResult()
        {
            // Arrange
            var time = DateTime.Now;
            var expirationTime = time.AddSeconds(SingleSignOnExpiredTime - 1);
            var user = new User
            {
                Id = 1,
                Username = "sa"
            };
            var model = new SingleSignOn
            {
                UserId = Cryptography.EncryptPassword("1"),
                OnUserId = Cryptography.EncryptPassword("2"),
                Time = Cryptography.EncryptPassword(time.ToLongTimeString())
            };
            model.Time = Cryptography.EncryptPassword(time.ToLongTimeString());
            var controller = GetController();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = MockServiceProvider()
                }
            };
            securityService.GetTime().Returns(Cryptography.EncryptPassword(expirationTime.ToLongTimeString()));
            userService.GetAsync(1).Returns(user);

            //Act
            var actual = await controller.SingleSignOn(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(actual);
            Assert.Equal(externalSiteHostUrl.DefaultPageUrl, redirectResult.Url);
            napSession.UserId = user.Id;
            napSession.UserName = user.Username;
            napSession.OnUserId = 2;
        }

        #endregion SingleSignOn

        [Fact]
        public async Task SignOut_WhenCalled_ReturnExpectedRedirectResult()
        {
            // Arrange
            var controller = GetController();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = MockServiceProvider()
                }
            };

            //Act
            var actual = await controller.SignOut();

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(actual);
            Assert.Equal(externalSiteHostUrl.SignInUrl, redirectResult.Url);
        }

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
                SingleSignOnExpiredTime = SingleSignOnExpiredTime
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

        private IServiceProvider MockServiceProvider()
        {
            var authServiceMock = Substitute.For<IAuthenticationService>();
            var serviceProviderMock = Substitute.For<IServiceProvider>();
            serviceProviderMock.GetService(typeof(IAuthenticationService))
                .Returns(authServiceMock);
            return serviceProviderMock;
        }

        #endregion Helpers
    }
}