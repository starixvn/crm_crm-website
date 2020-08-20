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
    using CRM.Models.NotificationList;
    using CRM.Models.NotificationType;
    using CRM.Services;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using FluentAssertions;
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
        private Fixture fixture = new Fixture();

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
            var notificationTypes = fixture.Create<List<NotificationType>>();
            var recipients = fixture.Create<List<BaseModel>>();
            var headlines = fixture.Create<List<BaseModel>>();
            var expected = new
            {
                NotificationTypes = notificationTypes.Select(type => new BaseModel { Id = type.NotificationTypeId, Name = type.NotificationTypeName }),
                Recipients = recipients,
                Headlines = headlines,
                Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(status => status.ToString())
            };

            var expectedResult = controller.Json(expected);
            notificationTypeService.GetNotificationTypeAsync(Arg.Any<GetNotificationTypeCriteria>()).Returns(notificationTypes);
            commonService.GetRecipients(Arg.Any<GetRecipientsCriteria>()).Returns(recipients);
            commonService.GetHeadlines(Arg.Any<GetHeadlinesCriteria>()).Returns(headlines);

            // Act
            var result = await controller.GetSettings() as JsonResult;

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetGridData_ToDateHasValue_ReturnExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var toDate = fixture.Create<DateTime>();
            var criteria = fixture.Create<GetNotificationListCriteria>();
            criteria.ToDate = toDate;
            var expectedToDate = toDate.AddDays(1).AddSeconds(-1);
            var notificationList = fixture.Create<IEnumerable<Notification>>();
            notificationListService.GetNotificationListAsync(criteria).Returns(notificationList);
            var expectedResult = controller.Json(new { totalItems = criteria.TotalRows, data = notificationList });

            // Act
            var result = await controller.GetGridData(criteria);

            // Assert
            Assert.Equal(expectedToDate, criteria.ToDate);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetGridData_ToDateNotHasValue_ReturnExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var criteria = fixture.Create<GetNotificationListCriteria>();
            criteria.ToDate = null;
            var notificationList = fixture.Create<IEnumerable<Notification>>();
            notificationListService.GetNotificationListAsync(criteria).Returns(notificationList);
            var expectedResult = controller.Json(new { totalItems = criteria.TotalRows, data = notificationList });

            // Act
            var result = await controller.GetGridData(criteria);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task EditNotificationList_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var command = fixture.Create<EditNotificationListCommand>();
            var commandResult = fixture.Create<CommandResult>();
            notificationListService.EditNotificationListAsync(command).Returns(commandResult);
            var expectedResult = controller.Json(new { resolvedBy = napSession.UserName, result = commandResult });

            // Act
            var result = await controller.EditNotificationList(command);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        //[Fact]
        //public async Task ValidateNotificationList_WhenCalled_ReturnExpectedResult()
        //{
        //    // Arrange
        //    var controller = GetController();
        //    var model = fixture.Create<ValidateNotificationListModel>();
        //    var expected = fixture.Create<ValidationNotificationListResult>();
        //    notificationListService.ValidateNotificationListAsync(Arg.Any<ValidateNotificationListCriteria>()).Returns(expected);
        //    expected.IsSingle = model.IsSingle;

        //    // Act
        //    var result = await controller.ValidateNotificationList(model);

        //    // Assert
        //    result.Should().BeEquivalentTo(expected);
        //}

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