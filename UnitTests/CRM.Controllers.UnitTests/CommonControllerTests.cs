namespace CRM.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Criteria.Common;
    using CRM.Criteria.NotificationType;
    using CRM.Models;
    using CRM.Models.Common;
    using CRM.Models.NotificationType;
    using CRM.Services;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;

    public class CommonControllerTests
    {
        private INotificationTypeService notificationTypeService;
        private ICommonService commonService;
        private readonly IFixture fixture = new Fixture();

        [Fact]
        public async Task GetNotificationTypes_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var expected = fixture.Create<IEnumerable<NotificationType>>();
            notificationTypeService.GetNotificationTypeAsync(Arg.Any<GetNotificationTypeCriteria>()).Returns(expected);

            // Action
            var result = await controller.GetNotificationTypes() as JsonResult;

            // Assert
            Assert.Equal(expected, (IEnumerable<NotificationType>)result.Value);
        }

        [Fact]
        public async Task GetHeadlines_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var criteria = fixture.Create<GetHeadlinesCriteria>();
            var expected = fixture.Create<IEnumerable<BaseModel>>();
            commonService.GetHeadlineAsync(criteria).Returns(expected);

            // Action
            var result = await controller.GetHeadlines(criteria) as JsonResult;

            // Assert
            Assert.Equal(expected, (IEnumerable<BaseModel>)result.Value);
        }

        [Fact]
        public async Task GetRecipients_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetController();
            var criteria = fixture.Create<GetRecipientsCriteria>();
            var expected = fixture.Create<IEnumerable<RecipientModel>>();
            commonService.GetRecipientAsync(criteria).Returns(expected);

            // Action
            var result = await controller.GetRecipients(criteria) as JsonResult;

            // Assert
            Assert.Equal(expected, (IEnumerable<RecipientModel>)result.Value);
        }

        private CommonController GetController()
        {
            notificationTypeService = Substitute.For<INotificationTypeService>();
            commonService = Substitute.For<ICommonService>();

            return new CommonController(notificationTypeService, commonService);
        }
    }
}