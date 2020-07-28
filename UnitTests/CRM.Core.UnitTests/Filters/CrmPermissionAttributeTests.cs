namespace CRM.Core.UnitTests.Filters
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using CRM.Core.Filters;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.Core.Business;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using NSubstitute;
    using Xunit;

    public class CrmPermissionAttributeTests
    {
        [Fact]
        public async Task OnAuthorizationAsync_NoPermission_ExpectedRedirectResult()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();
            var napSettings = Substitute.For<INapSettings>();
            var napSession = Substitute.For<INapSession>();

            userService.GetPermissionAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns((int)Constant.Policy.Read);

            var context = new AuthorizationFilterContext(
                new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                },
                new List<IFilterMetadata>());
            var filter = new CrmPermissionAttribute(userService, napSettings, napSession)
            {
                Policy = Constant.Policy.Host
            };

            // Act
            await filter.OnAuthorizationAsync(context);

            // Assert
            var redirectResult = Assert.IsAssignableFrom<RedirectToActionResult>(context.Result);
            Assert.Equal("error", redirectResult.ControllerName);
            Assert.Equal("405", redirectResult.ActionName);
        }

        [Fact]
        public async Task OnAuthorizationAsync_HavePermission_RequestSucceeded()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();
            var napSettings = Substitute.For<INapSettings>();
            var napSession = Substitute.For<INapSession>();

            userService.GetPermissionAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns((int)Constant.Policy.Host);

            var context = new AuthorizationFilterContext(
                new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                },
                new List<IFilterMetadata>());
            var filter = new CrmPermissionAttribute(userService, napSettings, napSession)
            {
                Policy = Constant.Policy.Host
            };

            // Act
            await filter.OnAuthorizationAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, context.HttpContext.Response.StatusCode);
        }
    }
}