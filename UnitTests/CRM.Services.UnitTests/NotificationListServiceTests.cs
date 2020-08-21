namespace CRM.Services.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationList;
    using CRM.Models.NotificationList;
    using CRM.Repositories;
    using NSubstitute;
    using Xunit;

    public class NotificationListServiceTests
    {
        private INotificationListRepository notificationListRepository;
        private IBaseService baseService;
        private readonly IFixture fixture = new Fixture();

        [Fact]
        public async Task GetNotificationListAsync_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var criteria = fixture.Create<GetNotificationListCriteria>();
            var expected = fixture.Create<IEnumerable<Notification>>();
            notificationListRepository.GetNotificationsAsync(criteria).Returns(expected);

            // Act
            var result = await service.GetNotificationListAsync(criteria);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task EditNotificationListAsync_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var command = fixture.Create<EditNotificationListCommand>();
            var expected = fixture.Create<CommandResult>();
            baseService.ExecuteNonQueryAsync(command).Returns(expected);

            // Act
            var result = await service.EditNotificationListAsync(command);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task ValidateNotificationListAsync_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var criteria = fixture.Create<ValidateNotificationListCriteria>();
            var expected = fixture.Create<ValidationNotificationListResult>();
            notificationListRepository.ValidateNotificationListAsync(criteria).Returns(expected);

            // Act
            var result = await service.ValidateNotificationListAsync(criteria);

            // Assert
            Assert.Equal(expected, result);
        }

        #region Helpers

        private NotificationListService GetService()
        {
            notificationListRepository = Substitute.For<INotificationListRepository>();
            baseService = Substitute.For<IBaseService>();
            var service = new NotificationListService(notificationListRepository, baseService);

            return service;
        }

        #endregion Helpers
    }
}