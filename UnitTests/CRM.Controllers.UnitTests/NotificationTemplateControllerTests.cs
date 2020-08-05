namespace CRM.Controllers.UnitTests
{
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Models.NotificationTemplate;
    using CRM.Services;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;

    public class NotificationTemplateControllerTests
    {
        private INotificationTemplateService templateService;

        [Fact]
        public async Task Create_WhenCalled_ReturnExpectedData()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<CommandResult>();
            var model = fixture.Create<CreateNotificationTemplate>();
            var controller = GetController();
            templateService.CreateAsync(model).Returns(expected);

            // Act
            var result = await controller.Create(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var commandResult = Assert.IsAssignableFrom<CommandResult>(jsonResult.Value);
            Assert.Equal(expected, commandResult);
        }

        #region Helpers

        private NotificationTemplateController GetController()
        {
            templateService = Substitute.For<INotificationTemplateService>();

            return new NotificationTemplateController(templateService);
        }

        #endregion Helpers
    }
}