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
        private ISettingService settingService;

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

        [Fact]
        public async Task Edit_WhenCalled_ReturnExpectedData()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<CommandResult>();
            var model = fixture.Create<EditNotificationTemplate>();
            var controller = GetController();
            templateService.EditAsync(model).Returns(expected);

            // Act
            var result = await controller.Edit(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var commandResult = Assert.IsAssignableFrom<CommandResult>(jsonResult.Value);
            Assert.Equal(expected, commandResult);
        }

        [Fact]
        public async Task Delete_WhenCalled_ReturnExpectedData()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<CommandResult>();
            var id = fixture.Create<int>();
            var controller = GetController();
            templateService.DeleteAsync(id).Returns(expected);

            // Act
            var result = await controller.Delete(id);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var commandResult = Assert.IsAssignableFrom<CommandResult>(jsonResult.Value);
            Assert.Equal(expected, commandResult);
        }

        #region Helpers

        private NotificationTemplateController GetController()
        {
            templateService = Substitute.For<INotificationTemplateService>();
            settingService = Substitute.For<ISettingService>();
            return new NotificationTemplateController(templateService, settingService);
        }

        #endregion Helpers
    }
}