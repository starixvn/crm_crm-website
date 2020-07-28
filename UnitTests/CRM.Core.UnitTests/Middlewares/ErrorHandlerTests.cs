namespace CRM.Core.UnitTests.Middlewares
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using CRM.Core.Middlewares;
    using Fanex.Logging;
    using Microsoft.AspNetCore.Http;
    using NSubstitute;
    using Xunit;

    public class ErrorHandlerTests
    {
        [Fact]
        public async Task InvokeAsync_Exception_ReturnAsExpected()
        {
            // Arrange
            var next = Substitute.For<RequestDelegate>();
            var logger = Substitute.For<ILogger>();
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Method = "post";
            httpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
            next.Invoke(Arg.Any<HttpContext>()).Returns(x => { throw new Exception(); });
            var middleware = new ErrorHandler(next, logger);

            // Act
            await middleware.InvokeAsync(httpContext);

            // Assert
            Assert.Equal(500, httpContext.Response.StatusCode);
            await logger.Received().ErrorAsync(Arg.Any<string>());
        }

        [Fact]
        public async Task InvokeAsync_RequestSucceeded_ReturnAsExpected()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var logger = Substitute.For<ILogger>();
            var next = Substitute.For<RequestDelegate>();
            var middleware = new ErrorHandler(next, logger);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, context.Response.StatusCode);
        }
    }
}