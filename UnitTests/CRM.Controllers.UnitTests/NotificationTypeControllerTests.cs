namespace CRM.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Models.NotificationType;
    using CRM.Services;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;
    public class NotificationTypeControllerTests
    {
        private INotificationTypeService notificationTypeService;
        private INapSession napSession;
        private INapSettings napSettings;
        private IUserService userService;
        private ICommonService commonService;

        [Fact]
        public async Task GetGridData_WhenCalled_ReturnExpectedData()
        {
            // Arrange
            var fixture = new Fixture();
            var types = fixture.Create<IEnumerable<NotificationType>>();
            var param = fixture.Create<GetGridDataParam>();
            var controller = GetController();
            //notificationTypeService.GetNotificationTypeAsync(param).Returns(types);

            // Act
            var result = await controller.GetGridData(param);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var notificationTypes = (IEnumerable<NotificationType>)jsonResult.Value.GetType().GetProperty("data").GetValue(jsonResult.Value);
            var totalItems = (int)jsonResult.Value.GetType().GetProperty("totalItems").GetValue(jsonResult.Value);
            Assert.Equal(types, notificationTypes);
            Assert.Equal(param.TotalRows, totalItems);
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
