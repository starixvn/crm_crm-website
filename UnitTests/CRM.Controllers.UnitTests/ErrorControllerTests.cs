namespace CRM.Controllers.UnitTests
{
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class ErrorControllerTests
    {
        [Fact]
        public void PageNotFound_WhenCalled_ReturnExpectedViewResult()
        {
            // Arrange
            var controller = new ErrorController();

            //Act
            var actual = controller.PageNotFound();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void AccessDenied_WhenCalled_ReturnExpectedViewResult()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var actual = controller.AccessDenied();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void InternalError_WhenCalled_ReturnExpectedViewResult()
        {
            // Arrange
            var controller = new ErrorController();

            // Act
            var actual = controller.InternalError();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            Assert.Null(viewResult.ViewName);
        }
    }
}