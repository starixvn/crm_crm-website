namespace CRM.Services.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationList;
    using CRM.Models;
    using CRM.Models.NotificationList;
    using NSubstitute;
    using Xunit;

    public class NotificationListServiceTests
    {
        private IBaseService baseService;
        private readonly IFixture fixture = new Fixture();

        [Fact]
        public async Task GetNotificationListAsync_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var criteria = fixture.Create<GetNotificationListCriteria>();
            var expected = fixture.Create<ValueTuple<IEnumerable<Notification>, IEnumerable<Recipient>, IEnumerable<Headline>>>();
            baseService.FetchMultipleAsync<Notification, Recipient, Headline>(criteria).Returns(expected);

            // Act
            var result = await service.GetNotificationListAsync(criteria);

            // Assert
            Assert.Equal(expected.Item1, result);
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
            var expected = fixture.Create<ValueTuple<IEnumerable<int>, IEnumerable<int>>>();
            baseService.FetchMultipleAsync<int, int>(criteria).Returns(expected);

            // Act
            var result = await service.ValidateNotificationListAsync(criteria);

            // Assert
            Assert.Equal(expected.Item1, result.InActiveNotificationIds);
            Assert.Equal(expected.Item2, result.ResendIds);
        }

        #region Helpers

        private NotificationListService GetService()
        {
            baseService = Substitute.For<IBaseService>();
            var service = new NotificationListService(baseService);

            return service;
        }

        #endregion Helpers
    }
}