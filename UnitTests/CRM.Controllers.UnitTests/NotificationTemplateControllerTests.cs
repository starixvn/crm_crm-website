namespace CRM.Controllers.UnitTests
{
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Constants;
    using CRM.Core.Models;
    using CRM.Models.NotificationTemplate;
    using CRM.Services;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;
    using static Fanex.NAP.Core.Business.Constant;

    public class NotificationTemplateControllerTests
    {
        private INapSession napSession;
        private INapSettings napSettings;
        private IUserService userService;
        private INotificationTemplateService templateService;
        private ISettingService settingService;

        [Fact]
        public void Index_WhenCalled_ReturnExpectedViewResult()
        {
            // Arrange
            var controller = GetController();
            userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationTemplate, Policy.Host).Returns(true);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<bool>(viewResult.Model);
            Assert.True(viewModel);
        }

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
            napSession = Substitute.For<INapSession>();
            napSettings = Substitute.For<INapSettings>();
            userService = Substitute.For<IUserService>();
            templateService = Substitute.For<INotificationTemplateService>();
            settingService = Substitute.For<ISettingService>();
            return new NotificationTemplateController(napSession, napSettings, userService, templateService, settingService);
        }

        #endregion Helpers
    }
}