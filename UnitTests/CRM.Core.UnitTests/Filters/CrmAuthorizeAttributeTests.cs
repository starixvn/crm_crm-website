namespace CRM.Core.UnitTests.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using CRM.Core.Filters;
    using Fanex.NAP.Client.Helpers.Web;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using NSubstitute;
    using Xunit;

    public class CrmAuthorizeAttributeTests
    {
        [Fact]
        public void OnAuthorization_UserIdAndUsernameAreInvalid_ExpectedRedirectResult()
        {
            // Arrange
            var napSession = Substitute.For<INapSession>();
            var authService = Substitute.For<IAuthenticationService>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IAuthenticationService)).Returns(authService);
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };
            var context = new AuthorizationFilterContext(
                new ActionContext
                {
                    HttpContext = httpContext,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                },
                new List<IFilterMetadata>());
            var filter = new CrmAuthorizeAttribute(napSession);

            // Act
            filter.OnAuthorization(context);

            // Assert
            var redirectResult = Assert.IsAssignableFrom<RedirectToActionResult>(context.Result);
            Assert.Equal("Auth", redirectResult.ControllerName);
            Assert.Equal("SignOut", redirectResult.ActionName);
        }

        [Fact]
        public void OnAuthorization_UserIdAndUsernameAreValid_RequestSucceeded()
        {
            // Arrange
            var napSession = Substitute.For<INapSession>();
            napSession.UserId = 1;
            napSession.UserName = "tim.trinh";

            var authService = Substitute.For<IAuthenticationService>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IAuthenticationService)).Returns(authService);

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };
            var context = new AuthorizationFilterContext(
                new ActionContext
                {
                    HttpContext = httpContext,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                },
                new List<IFilterMetadata>());
            var filter = new CrmAuthorizeAttribute(napSession);

            // Act
            filter.OnAuthorization(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, context.HttpContext.Response.StatusCode);
        }
    }
}