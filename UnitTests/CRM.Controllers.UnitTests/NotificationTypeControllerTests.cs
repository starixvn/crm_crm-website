namespace CRM.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Constants;
    using CRM.Core.Models;
    using CRM.Criteria.NotificationType;
    using CRM.Models;
    using CRM.Models.NotificationType;
    using CRM.Services;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;
    using static Fanex.NAP.Core.Business.Constant;

    public class NotificationTypeControllerTests
    {
        private INotificationTypeService notificationTypeService;
        private INapSession napSession;
        private INapSettings napSettings;
        private IUserService userService;
        private ICommonService commonService;
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public async Task GetSettings_WhenCalled_ReturnExpectedViewResult()
        {
            //Arrange
            var controller = GetController();
            var chatTypes = fixture.Create<IEnumerable<BaseModel>>();
            var systems = fixture.Create<IEnumerable<BaseModel>>();
            var recipientTypes = fixture.Create<IEnumerable<BaseModel>>();
            commonService.GetRecipientTypeAsync().Returns(recipientTypes);
            commonService.GetSystemAsync().Returns(systems);
            commonService.GetChatTypes().Returns(chatTypes);

            //Act
            var result = await controller.GetSettings() as JsonResult;

            // Assert
            var chatTypesResult = (IEnumerable<BaseModel>)result.Value.GetType().GetProperty("ChatTypes").GetValue(result.Value);
            var systemsResult = (IEnumerable<BaseModel>)result.Value.GetType().GetProperty("Systems").GetValue(result.Value);
            var recipientResult = (IEnumerable<BaseModel>)result.Value.GetType().GetProperty("RecipientTypes").GetValue(result.Value);
            Assert.Equal(chatTypes, chatTypesResult);
            Assert.Equal(systems, systemsResult);
            Assert.Equal(recipientTypes, recipientResult);
        }

        [Fact]
        public void Index_WhenCalled_ReturnExpectedViewResult()
        {
            // Arrange
            var controller = GetController();
            userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationType, Policy.Host).Returns(true);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<bool>(viewResult.Model);
            Assert.True(viewModel);
        }

        [Fact]
        public async Task GetGridData_WhenCalled_ReturnExpectedData()
        {
            // Arrange
            var notificationTypes = fixture.Create<IEnumerable<NotificationType>>();
            var param = fixture.Create<GetNotificationTypeParam>();
            var criteria = new GetNotificationTypeCriteria();
            var controller = GetController();
            notificationTypeService.GetNotificationTypeAsync(Arg.Do<GetNotificationTypeCriteria>(x => criteria = x)).Returns(notificationTypes);

            // Act
            var result = await controller.GetNotificationType(param);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var notificationTypeResult = (IEnumerable<NotificationType>)jsonResult.Value.GetType().GetProperty("data").GetValue(jsonResult.Value);
            var totalItems = (int)jsonResult.Value.GetType().GetProperty("totalItems").GetValue(jsonResult.Value);
            Assert.Equal(notificationTypes, notificationTypeResult);
            Assert.Equal(criteria.TotalRows, totalItems);
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnExpectedData()
        {
            // Arrange
            var expected = fixture.Create<CommandResult>();
            var model = fixture.Create<CreateNotificationType>();
            var controller = GetController();
            notificationTypeService.CreateAsync(model).Returns(expected);

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
            var expected = fixture.Create<CommandResult>();
            var model = fixture.Create<EditNotificationType>();
            var controller = GetController();
            notificationTypeService.EditAsync(model).Returns(expected);

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
            var expected = fixture.Create<CommandResult>();
            var model = fixture.Create<DeleteNotificationType>();
            var id = fixture.Create<int>();
            var controller = GetController();
            notificationTypeService.DeleteAsync(model).Returns(expected);

            // Act
            var result = await controller.Delete(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var commandResult = Assert.IsAssignableFrom<CommandResult>(jsonResult.Value);
            Assert.Equal(expected, commandResult);
        }

        private NotificationTypeController GetController()
        {
            notificationTypeService = Substitute.For<INotificationTypeService>();
            napSession = Substitute.For<INapSession>();
            napSettings = Substitute.For<INapSettings>();
            userService = Substitute.For<IUserService>();
            commonService = Substitute.For<ICommonService>();
            return new NotificationTypeController(notificationTypeService, napSession, napSettings, userService, commonService);
        }
    }
}
