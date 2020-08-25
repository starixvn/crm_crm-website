namespace CRM.Repositories.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Models;
    using CRM.Models.NotificationList;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    public class NotificationListRepositoryTests
    {
        private readonly IObjectDb objectDb;
        private readonly IFixture fixture = new Fixture();

        public NotificationListRepositoryTests()
        {
            DbSettingProviderManager
            .StartTestingSession()
            .Run();

            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((key, time) => objectDb);
        }

        [Fact]
        public async Task GetNotificationsAsync_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var repository = new NotificationListRepository();
            var criteria = Substitute.For<ICriteria>();
            criteria.GetSettingKey().Returns("CRM_HeadlineNotification_Test");
            var notifications = fixture.Create<IEnumerable<Notification>>();
            var recipients = fixture.Create<IEnumerable<Recipient>>();
            var headlines = fixture.Create<IEnumerable<Headline>>();
            var multi = Substitute.For<IMultiResultReader>();
            multi.Read().Returns(notifications);
            multi.Read().Returns(recipients);
            multi.Read().Returns(headlines);
            objectDb.QueryMultipleAsync(criteria).Returns(multi);

            // Action
            var result = await repository.GetNotificationsAsync(criteria);

            // Assert
            Assert.Equal(notifications, result);
        }

        [Fact]
        public async Task ValidateNotificationListAsync_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var repository = new NotificationListRepository();
            var criteria = Substitute.For<ICriteria>();
            var multi = Substitute.For<IMultiResultReader>();
            //var inActiveNotificationIds = new List<object> { 1, 2, 3 };
            var resendDataList = fixture.Create<IEnumerable<NotificationListResend>>();
            var expected = new ValidationNotificationListResult
            {
                //InActiveNotificationIds = (List<int>)inActiveNotificationIds,
                ResendDataList = resendDataList
            };

            //multi.Read().Returns(inActiveNotificationIds);
            multi.Read().Returns(resendDataList);
            objectDb.QueryMultipleAsync(criteria).Returns(multi);

            // Action
            var result = await repository.ValidateNotificationListAsync(criteria);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}