namespace CRM.Controllers.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Constants;
    using CRM.Core.Enumerables;
    using CRM.Core.Models;
    using CRM.Criteria.Common;
    using CRM.Criteria.NotificationList;
    using CRM.Criteria.NotificationType;
    using CRM.Models;
    using CRM.Models.Common;
    using CRM.Models.NotificationList;
    using CRM.Models.NotificationType;
    using CRM.Services;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;
    using static Fanex.NAP.Core.Business.Constant;

    public class NotificationListControllerTests
    {
        private INapSession napSession;
        private INapSettings napSettings;
        private IUserService userService;
        private INotificationListService notificationListService;
        private INotificationTypeService notificationTypeService;
        private ICommonService commonService;
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public void Index_WhenCalled_ReturnExpectedViewResult()
        {
            // Arrange
            var controller = GetController();
            userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationList, Policy.Host).Returns(true);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<bool>(viewResult.Model);
            Assert.True(viewModel);
        }

        [Fact]
        public async Task GetSettings_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var notificationTypes = fixture.Create<IEnumerable<NotificationType>>();
            var statuses = Enum.GetValues(typeof(NotificationStatus)).Cast<NotificationStatus>().Select(status => status.ToString());
            notificationTypeService.GetNotificationTypeAsync(Arg.Any<GetNotificationTypeCriteria>()).Returns(notificationTypes);

            // Act
            var result = await controller.GetSettings() as JsonResult;

            // Assert
            var notificationTypesResult = (IEnumerable<NotificationType>)result.Value.GetType().GetProperty("NotificationTypes").GetValue(result.Value);
            var statusesResult = (IEnumerable<string>)result.Value.GetType().GetProperty("Statuses").GetValue(result.Value);
            Assert.Equal(notificationTypes, notificationTypesResult);
            Assert.Equal(statuses, statusesResult);
        }

        [Fact]
        public async Task GetGridData_FromDateAndToDateIsNull_ReturnExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var criteria = fixture.Create<GetNotificationListCriteria>();
            criteria.FromDate = null;
            criteria.ToDate = null;
            var notificationList = fixture.Create<IEnumerable<Notification>>();
            notificationListService.GetNotificationListAsync(criteria).Returns(notificationList);

            // Act
            var result = await controller.GetGridData(criteria) as JsonResult;

            // Assert
            var totalItemsResult = (int)result.Value.GetType().GetProperty("totalItems").GetValue(result.Value);
            var dataResult = (IEnumerable<Notification>)result.Value.GetType().GetProperty("data").GetValue(result.Value);
            Assert.Equal(criteria.TotalRows, totalItemsResult);
            Assert.Equal(notificationList, dataResult);
        }

        [Fact]
        public async Task GetGridData_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var criteria = fixture.Create<GetNotificationListCriteria>();
            var expectedDate = fixture.Create<DateTime>();
            var notificationList = fixture.Create<IEnumerable<Notification>>();
            commonService.ConvertClientToServerDateTime(Arg.Any<DateTime>()).Returns(expectedDate);
            notificationListService.GetNotificationListAsync(criteria).Returns(notificationList);

            // Act
            var result = await controller.GetGridData(criteria) as JsonResult;

            // Assert
            var totalItemsResult = (int)result.Value.GetType().GetProperty("totalItems").GetValue(result.Value);
            var dataResult = (IEnumerable<Notification>)result.Value.GetType().GetProperty("data").GetValue(result.Value);
            Assert.Equal(criteria.TotalRows, totalItemsResult);
            Assert.Equal(notificationList, dataResult);
            Assert.Equal(expectedDate, criteria.FromDate.Value);
            Assert.Equal(expectedDate, criteria.ToDate.Value);
        }

        [Fact]
        public async Task EditNotificationList_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var command = fixture.Create<EditNotificationListCommand>();
            var commandResult = fixture.Create<CommandResult>();
            notificationListService.EditNotificationListAsync(command).Returns(commandResult);

            // Act
            var result = await controller.EditNotificationList(command) as JsonResult;

            // Assert
            var resolvedByResult = (string)result.Value.GetType().GetProperty("resolvedBy").GetValue(result.Value);
            var actualCommandResult = (CommandResult)result.Value.GetType().GetProperty("result").GetValue(result.Value);
            Assert.Equal(napSession.UserName, resolvedByResult);
            Assert.Equal(commandResult, actualCommandResult);
        }

        [Fact]
        public async Task ValidateNotificationList_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var model = fixture.Create<ValidateNotificationListModel>();
            var expected = fixture.Create<ValidationNotificationListResult>();
            notificationListService.ValidateNotificationListAsync(Arg.Any<ValidateNotificationListCriteria>()).Returns(expected);
            expected.IsSingle = model.IsSingle;

            // Act
            var result = await controller.ValidateNotificationList(model) as JsonResult;

            // Assert
            var valueResult = (ValidationNotificationListResult)result.Value;
            Assert.Equal(expected, valueResult);
        }

        [Fact]
        public async Task ResendNotificationList_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var command = fixture.Create<ResendNotificationsCommand>();
            var expected = fixture.Create<CommandResult>();
            notificationListService.ResendNotificationsAsync(command).Returns(expected);

            // Act
            var result = await controller.ResendNotificationList(command) as JsonResult;

            // Assert
            var valueResult = (CommandResult)result.Value;
            Assert.Equal(napSession.UserId, command.UserId);
            Assert.Equal(expected, valueResult);
        }

        #region Helpers

        private NotificationListController GetController()
        {
            napSession = Substitute.For<INapSession>();
            napSettings = Substitute.For<INapSettings>();
            userService = Substitute.For<IUserService>();
            notificationListService = Substitute.For<INotificationListService>();
            notificationTypeService = Substitute.For<INotificationTypeService>();
            commonService = Substitute.For<ICommonService>();

            return new NotificationListController(napSession, napSettings, userService, notificationListService, notificationTypeService, commonService);
        }

        #endregion Helpers
    }
}