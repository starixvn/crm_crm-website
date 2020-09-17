namespace CRM.BackgroundJob.UnitTests.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Controllers;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class AuthControllerTests
    {
        private HangfireAuthentication auth;

        [Fact]
        public void Login_WhenCalled_ReturnExpectedViewResult()
        {
            // Arrange
            var controller = GetController();

            //Act
            var actual = controller.Login();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.Null(viewResult.ViewName);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "1234aa")]
        [InlineData("starix", "")]
        public async Task Login_UsernameAndPasswordAreInvalid_ReturnExpectedViewResult(string username, string password)
        {
            // Arrange
            var controller = GetController();

            //Act
            var actual = await controller.Login(username, password);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.Null(viewResult.ViewName);
            var isValid = Assert.IsAssignableFrom<bool>(viewResult.ViewData["IsValid"]);
            Assert.False(isValid);
        }

        [Fact]
        public async Task Login_UsernameAndPasswordAreValid_ReturnExpectedRedirectResult()
        {
            // Arrange
            var username = "starix";
            var password = "1234aa";
            var controller = GetController();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = MockServiceProvider()
                }
            };

            //Act
            var actual = await controller.Login(username, password);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(actual);
            Assert.Equal("~/hangfire", redirectResult.Url);
        }

        [Fact]
        public async Task Logout_WhenCalled_ReturnExpectedRedirectResult()
        {
            // Arrange
            var controller = GetController();
            controller.Url = Substitute.For<IUrlHelper>();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = MockServiceProvider()
                }
            };

            //Act
            var actual = await controller.Logout();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(actual);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Login", redirectResult.ActionName);
        }

        [Fact]
        public void Error_WhenCalled_ReturnExpectedViewResult()
        {
            // Arrange
            var controller = GetController();

            //Act
            var actual = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.Null(viewResult.ViewName);
        }

        #region Helpers

        private AuthController GetController()
        {
            var settings = Substitute.For<IOptionsSnapshot<HangfireAuthentication>>();
            auth = new HangfireAuthentication
            {
                Username = "starix",
                Password = Cryptography.EncryptPassword("1234aa")
            };

            settings.Value.Returns(auth);
            return new AuthController(settings);
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